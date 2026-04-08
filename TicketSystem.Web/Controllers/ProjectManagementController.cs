using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketSystem.Web.Models;
using TicketSystem.Web.Models.Account;
using TicketSystem.Web.Models.ProjectManagement;
using TicketSystem.Web.Models.Users;
using TicketSystem.Web.Models.Workflow;

namespace TicketSystem.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProjectManagementController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectManagementController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            var viewModel = await BuildViewModelAsync();
            return View(viewModel);
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.Projects
                .Include(p => p.CreatedBy)
                .Include(p => p.Members)
                    .ThenInclude(pm => pm.Member)
                .Include(p => p.Workflow)
                    .ThenInclude(w => w!.Statuses) 
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);


            if (project == null) return NotFound();

            var existingMemberIds = project.Members.Select(m => m.MemberId).ToList();
            var availableUsers = await _context.Users
                .Where(u => u.IsActive && !existingMemberIds.Contains(u.Id))
                .Select(u => new SelectListItem { Value = u.Id, Text = u.Name })
                .ToListAsync();

            var availableRoles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Manager", Text = "Manager" },
                new SelectListItem { Value = "Member", Text = "Member" }
            };

            var viewModel = new ProjectDetailsViewModel
            {
                // Home
                ProjectId = project.Id,
                Title = project.Title,
                Description = project.Description,
                CreatedByName = project.CreatedBy?.Name ?? "Unknown",
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                TotalTickets = project.Tickets?.Count() ?? 0,
                TotalOpenedTickets = project.Tickets?.Count(t => t.CurrentStatus != "Closed") ?? 0,

                // Members
                ExistingMembers = project.Members.Select(m => new ProjectMemberItemViewModel
                {
                    MemberId = m.MemberId,
                    MemberName = m.Member?.Name ?? "Unknown",
                    RoleInProject = m.RoleInProject,
                    Initials = AvatarHelper.GetInitials(m.Member?.Name ?? "Default")
                }).ToList(),
                AddMemberForm = new AddProjectMemberViewModel
                {
                    ProjectId = project.Id,
                    AvailableUsers = availableUsers,
                    AvailableRoles = availableRoles
                },

                // Workflow
                WorkflowId = project.WorkflowId,
                WorkflowName = project.Workflow?.Name ?? "Default Workflow", 
                WorkflowStatuses = project.Workflow!.Statuses!.Select(s => new WorkflowStatusItemViewModel
                {
                    StatusId = s.Id,
                    Name = s.Name,
                    IsInicial = s.IsInicial,
                    IsFinal = s.IsFinal
                }).ToList(),
                AddStatusForm = new AddWorkflowStatusViewModel
                {
                    ProjectId = project.Id,
                    WorkflowId = project.WorkflowId
                }
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(CreateProjectVM CreateForm)
        {

            if (!ModelState.IsValid)
                return View("Index", await BuildViewModelAsync(CreateForm, showCreateModal: true));

            // Workflow Statuses
            var Statuses = new List<WorkflowStatus>
            {
                new WorkflowStatus { Name = "Open", IsInicial = true, IsFinal = false },
                new WorkflowStatus { Name = "Closed", IsInicial = false, IsFinal = true }
            };

            // Workflow
            var newWorkflow = new WorkflowModel
            {
                Name = $"{CreateForm.Title} - Workflow",
                Statuses = new List<WorkflowStatus>
                {
                    new WorkflowStatus { Name = "Open", IsInicial = true, IsFinal = false },
                    new WorkflowStatus { Name = "Closed", IsInicial = false, IsFinal = true }
                }
            };

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var project = new ProjectModel
            {
                Title = CreateForm.Title,
                Description = CreateForm.Description,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                CreatedById = currentUserId!,
                Workflow = newWorkflow,
                IsDeleted = false
            };

            // Create Membership
            var membership = new ProjectMember
            {
                Project = project,
                MemberId = currentUserId!,
                RoleInProject = "Manager"
            };

            _context.Add(project);
            _context.Add(membership);
            await _context.SaveChangesAsync();
            await BuildViewModelAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null) return NotFound();

            var editForm = new EditProjectVM
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description
            };

            var viewModel = await BuildViewModelAsync(editForm: editForm, showEditModal: true);
            return View("Index", viewModel);

        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditProjectVM editForm)
        {
            if (id != editForm.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                return View("Index", await BuildViewModelAsync(editForm: editForm, showEditModal: true));
            }

            var projectModel = await _context.Projects.FindAsync(editForm.Id);

            if (projectModel == null) return NotFound();

            projectModel.Title = editForm.Title;
            projectModel.Description = editForm.Description;

            try
            {
                _context.Update(projectModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectModelExists(projectModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectModel = await _context.Projects
                .Include(p => p.Workflow)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectModel = await _context.Projects.FindAsync(id);
            
            if (projectModel != null)
            {
                projectModel.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectModelExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }


        private async Task<PMCreateEditViewModel> BuildViewModelAsync(
                                                        CreateProjectVM? createForm = null,
                                                        EditProjectVM? editForm = null,
                                                        bool showCreateModal = false,
                                                        bool showEditModal = false)
        {
            var projects = await _context.Projects.Select(p => new ProjectListVM
            {
                Id = p.Id,
                Title = p.Title,
                StartDate = p.StartDate,
                EndDate =  p.EndDate,
                CreatedBy = p.CreatedBy!.UserName ?? "Unknown"
            }).ToListAsync();

            return new PMCreateEditViewModel
            {
                Projects = projects,
                CreateForm = createForm ?? new CreateProjectVM(),
                EditForm = editForm ?? new EditProjectVM(),
                ShowCreateModal = showCreateModal,
                ShowEditModal = showEditModal
            };
        }

    }
}

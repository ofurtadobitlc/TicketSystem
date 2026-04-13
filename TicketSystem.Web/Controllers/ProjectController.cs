using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TicketSystem.Web.Models;
using TicketSystem.Web.Models.Project;
using TicketSystem.Web.Models.ProjectManagement;
using TicketSystem.Web.Models.Ticket;

namespace TicketSystem.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }


        // GET: Project/Index
        public async Task<IActionResult> Index()
        {
            var projectsList = await _context.Projects
            .Where(p => !p.IsDeleted)
            .Select(p => new ProjectListViewModel
            {
                Id = p.Id,
                Title = p.Title,
                DescriptionSnippet = p.Description.Length > 100 ? p.Description.Substring(0, 100) + "..." : p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                WorkflowName = p.Workflow != null ? p.Workflow.Name : "Without Workflow",
                TotalTickets = p.Tickets.Count(),
                OpenTickets = p.Tickets.Count(t => t.CurrentStatus != "Closed")
            }).ToListAsync();

            return View(projectsList);
        }

        // GET: Project/Details/5 (Kanban board)
        public async Task<IActionResult> Details(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Workflow)
                .ThenInclude(w => w.Statuses)
                .Include(p => p.Tickets)
                .ThenInclude(t => t.Assignee)
                .Include(p => p.Tickets).ThenInclude(t => t.Comments)
                .Include(p => p.Tickets).ThenInclude(t => t.BlockedByTickets)
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null) return NotFound();

            var boardViewModel = new ProjectBoardViewModel
            {
                ProjectId = project.Id,
                ProjectTitle = project.Title,
                WorkflowName = project.Workflow?.Name ?? "",
                EndDate = project.EndDate,
                WorkflowStatuses = project.Workflow?.Statuses
                                                .OrderByDescending(s => s.IsInicial)
                                                .ThenBy(s => s.IsFinal)
                                                .ThenBy(s => s.OrderIndex)
                                                .Select(s => s.Name).ToList() ?? new List<string>(),
                Tickets = project.Tickets.Select(t => new TicketCardViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    CurrentStatus = t.CurrentStatus,
                    AssigneeName = t.Assignee?.UserName ?? "Not assigned",
                    CommentsCount = t.Comments.Count,
                    AttachmentsCount = t.Attachments?.Count ?? 0,
                    IsClosed = t.ClosedAt.HasValue,
                    IsBlocked = t.BlockedByTickets.Any(),
                    IsLocked = !CanChangeTicketStatus(t, project),
                    CanAssign = CanAssignTicket(t, project)
                }).ToList(),
                CanCreateTicket = CanCreateTicket(project)
                
            };

            boardViewModel.UsersList = new SelectList(_context.Users.ToList(), "Id", "UserName");


            return View(boardViewModel);
        }


        // TODO: MOVE TO PROJECT MANAGEMENT
        // POST: Project/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null) return NotFound();

            // Dupla verificação de segurança no backend
            if (!project.EndDate.HasValue)
            {
                TempData["ErrorMessage"] = "Only closed Projects can be deleted";
                return RedirectToAction(nameof(Index));
            }

            // Soft Delete
            project.IsDeleted = true;
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"The Project '{project.Title}' was successfully deleted.";
            return RedirectToAction(nameof(Index));
        }

        private bool CanChangeTicketStatus(TicketModel ticket, ProjectModel project)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!project.EndDate.HasValue && ticket.AssigneeId != null && ticket.CurrentStatus != "Closed" && !ticket.BlockedByTickets.Any() && (User.IsInRole("Admin") || project.Members.Any(pm => pm.RoleInProject == "Manager" && pm.MemberId == currentUserId) || currentUserId == ticket.CreatorId || currentUserId == ticket.AssigneeId))
            {
                return true;
            }
            return false;
        }

        private bool CanCreateTicket(ProjectModel project)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!project.EndDate.HasValue && (User.IsInRole("Admin") || project.Members.Any(pm => pm.MemberId == currentUserId)))
            {
                return true;
            }
            return false;
        }

        private bool CanAssignTicket(TicketModel ticket, ProjectModel project)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!project.EndDate.HasValue && ticket.CurrentStatus != "Closed" && (User.IsInRole("Admin") || project.Members.Any(pm => pm.RoleInProject == "Manager" && pm.MemberId == currentUserId) || currentUserId == ticket.CreatorId))
            {
                return true;
            }
            return false;
        }
    }
}

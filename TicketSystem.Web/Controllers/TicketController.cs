using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketSystem.Web.Models;
using TicketSystem.Web.Models.Project;
using TicketSystem.Web.Models.Ticket;

namespace TicketSystem.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Ticket
        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Tickets
                                    .Include(t => t.Assignee)
                                    .Include(t => t.ClosedBy)
                                    .Include(t => t.CreatedBy)
                                    .Include(t => t.Project)
                                    .Select(t => new DisplayTicketViewModel
                                    {
                                        Id = t.Id,
                                        Title = t.Title,
                                        Description = t.Description,
                                        Project = t.Project != null ? t.Project.Title : "No Project",
                                        CreatedBy = t.CreatedBy != null ? t.CreatedBy.UserName! : "Unknown",
                                        CreatedAt = t.CreatedAt,
                                        Assignee = t.Assignee != null ? t.Assignee.UserName! : "Not Assigned",
                                        AssignedAt = t.AssignedAt,
                                        ClosedBy = t.ClosedBy != null ? t.ClosedBy.UserName! : "-",
                                        ClosedAt = t.ClosedAt,
                                        CurrentStatus = t.CurrentStatus
                                    }).ToListAsync();


            return View(tickets);
        }

        // GET: Ticket/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketModel = await _context.Tickets
                .Include(t => t.Assignee)
                .Include(t => t.ClosedBy)
                .Include(t => t.CreatedBy)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketModel == null)
            {
                return NotFound();
            }

            return View(ticketModel);
        }

        // GET: Ticket/Create
        public async Task<IActionResult> Create()
        {
            var users = await _context.Users.ToListAsync();
            var projects = await _context.Projects.ToListAsync();

            var viewModel = new CreateTicketViewModel
            {
                Projects = projects.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Title
                }),
            };
            return View(viewModel);
        }

        // POST: Ticket/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTicketViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects.FindAsync(viewModel.ProjectId);
            if (projects == null)
            {
                return NotFound();
            }

            
            /* TO MOVE
            var workflowStatuses = await _context.WorkflowStatuses.ToListAsync();

            var wfStatusProject = workflowStatuses.Where(w => w.WorkflowId == projects.WorkflowId);

            string firstStatus = wfStatusProject.Where(s => s.IsInicial).Select(s => s.Name).ToList()[0];*/


            if (ModelState.IsValid)
            {
                

                var ticket = new TicketModel
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    ProjectId = viewModel.ProjectId,
                    CreatorId = userId,
                    CreatedAt = DateTime.Now,
                    CurrentStatus = "Open"
                };

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.Projects = new SelectList(_context.Projects, "Id", "Title", viewModel.ProjectId);

            return View(viewModel);
        }

        // GET: Ticket/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketModel = await _context.Tickets
                            .Include(t => t.Project)
                            .Include(t => t.CreatedBy)
                            .FirstOrDefaultAsync(m => m.Id == id); 
            if (ticketModel == null)
            {
                return NotFound();
            }
            ViewData["AssigneeId"] = new SelectList(_context.Users, "Id", "UserName", ticketModel.AssigneeId);
            return View(ticketModel);
        }

        // POST: Ticket/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,AssigneeId")] TicketModel ticketModel)
        {
            if (id != ticketModel.Id)
            {
                return NotFound();
            }

            var ticketFromDB = await _context.Tickets.FindAsync(id);

            if (ticketFromDB == null)
            {
                return NotFound();
            }

            ticketFromDB.Description = ticketModel.Description;
            ticketFromDB.AssigneeId = ticketModel.AssigneeId;
            ticketFromDB.AssignedAt = DateTime.Now;

            ModelState.Clear();

            TryValidateModel(ticketFromDB);

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketModelExists(ticketModel.Id))
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
            ViewData["AssigneeId"] = new SelectList(_context.Users, "Id", "Id", ticketModel.AssigneeId);

            return View(ticketModel);
        }

        // GET: Ticket/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketModel = await _context.Tickets
                .Include(t => t.Assignee)
                .Include(t => t.ClosedBy)
                .Include(t => t.CreatedBy)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketModel == null)
            {
                return NotFound();
            }

            return View(ticketModel);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketModel = await _context.Tickets.FindAsync(id);
            if (ticketModel != null)
            {
                _context.Tickets.Remove(ticketModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketModelExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}

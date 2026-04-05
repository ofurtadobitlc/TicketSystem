using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Web.Models.Ticket
{
    public class TicketsListCreateEditViewModel
    {
        public List<TicketListViewModel> Tickets { get; set; } = [];
        public TicketCreateViewModel CreateForm { get; set; } = new();
        public TicketEditViewModel EditForm { get; set; } = new();
        public bool ShowCreateModal { get; set; } = false;
        public bool ShowEditModal { get; set; } = false;
    }

    public class TicketListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Project { get; set; } = string.Empty;
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; } = string.Empty;
        [Display(Name = "Created At")]
        public DateOnly CreatedAt { get; set; }
        public string Assignee { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public string CurrentStatus { get; set; } = string.Empty;
        public bool CanChange { get; set; } = false;

    }

    public class TicketCreateViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Project")]
        [Required(ErrorMessage = "Project is required.")]
        public int ProjectId { get; set; }
        public IEnumerable<SelectListItem>? Projects { get; set; }

        [Display(Name = "Assign to")]
        public string? AssigneeId { get; set; }
        public IEnumerable<SelectListItem>? UsersList { get; set; }
    }

    public class TicketEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; } 
        public IEnumerable<SelectListItem>? StatusList { get; set; }

        [Display(Name = "Assign to")]
        public string? AssigneeId { get; set; }
        public IEnumerable<SelectListItem>? UsersList { get; set; }
    }
}

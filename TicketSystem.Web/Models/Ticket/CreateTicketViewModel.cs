using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketSystem.Web.Models.Account;
using TicketSystem.Web.Models.Project;

namespace TicketSystem.Web.Models.Ticket
{
    public class CreateTicketViewModel
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
        public SelectList? UsersList { get; set; }
    }
}

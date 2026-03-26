using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketSystem.Web.Models.Account;
using TicketSystem.Web.Models.Project;

namespace TicketSystem.Web.Models.Ticket
{
    public class CreateTicketViewModel
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        public IEnumerable<SelectListItem>? Projects { get; set; }
    }
}

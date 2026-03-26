using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketSystem.Web.Models.Ticket
{
    public class EditTicketViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Project { get; set; } = string.Empty;
        public string Creator { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? AssignedAt { get; set; }
        public string AssigneeId { get; set; } = string.Empty;
        public string CurrentStatus { get; set; } = string.Empty;

        public IEnumerable<SelectListItem>? Users { get; set; }
    }
}

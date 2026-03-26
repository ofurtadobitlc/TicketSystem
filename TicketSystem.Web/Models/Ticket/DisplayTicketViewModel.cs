using TicketSystem.Web.Models.Account;

namespace TicketSystem.Web.Models.Ticket
{
    public class DisplayTicketViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Project { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Assignee { get; set; } = string.Empty;
        public DateTime? AssignedAt { get; set; }
        public string ClosedBy { get; set; } = string.Empty;
        public DateTime? ClosedAt { get; set; }
        public string CurrentStatus { get; set; } = string.Empty;

    }
}

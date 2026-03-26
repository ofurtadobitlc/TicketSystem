using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketSystem.Web.Models.Account;
using TicketSystem.Web.Models.Project;

namespace TicketSystem.Web.Models.Ticket
{
    public class TicketModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }

        public required string Description { get; set; }
        [ForeignKey("ProjectModel")]
        public int ProjectId { get; set; }

        [ForeignKey("AppUser")]
        public required string CreatorId { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("AppUser")]
        public string? AssigneeId { get; set; }

        public DateTime? AssignedAt { get; set; }
        [ForeignKey("AppUser")]
        public string? ClosedById { get; set; }

        public DateTime? ClosedAt { get; set; }

        public required string CurrentStatus { get; set; }

        public AppUser? CreatedBy { get; set; }
        public ProjectModel? Project { get; set; }

        public AppUser? Assignee { get; set; }
        
        public AppUser? ClosedBy { get; set; }

        public virtual ICollection<TicketAttachment> Attachments { get; set; } = [];
        public virtual ICollection<TicketComment> Comments { get; set; } = [];

        public virtual ICollection<TicketDependency> BlockedByTickets { get; set; } = [];
        public virtual ICollection<TicketDependency> BlockingTickets { get; set; } = [];


    }
}

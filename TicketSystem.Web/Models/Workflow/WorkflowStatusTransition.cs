using System.ComponentModel.DataAnnotations.Schema;
using TicketSystem.Web.Models.Account;

namespace TicketSystem.Web.Models.Workflow
{
    public class WorkflowStatusTransition
    {
        public int Id { get; set; }
        public int FromStatusId { get; set; }
        public WorkflowStatus? FromStatus { get; set; }
        public int ToStatusId { get; set; }
        public WorkflowStatus? ToStatus { get; set; }
        [ForeignKey("AppRole")]
        public required string AuthorizedRoleId { get; set; }
        public AppRole? AuthorizedRole { get; set; }
    }
}

using TicketSystem.Web.Models.ProjectManagement;
using TicketSystem.Web.Models.Ticket;

namespace TicketSystem.Web.Models.Workflow
{
    public class WorkflowModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<ProjectModel> Projects { get; set; } = [];
        public virtual ICollection<WorkflowStatus> Statuses { get; set; } = [];
    }
}

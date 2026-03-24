namespace TicketSystem.Web.Models.Workflow
{
    public class WorkflowStatusTransition
    {
        public int Id { get; set; }
        public int FromStatusId { get; set; }
        public required string RoleId { get; set; }
    }
}

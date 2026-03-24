namespace TicketSystem.Web.Models.Workflow
{
    public class WorkflowStatus
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public required string Name { get; set; }
        public bool IsInicial { get; set; }
        public bool IsFinal { get; set; }
    }
}

namespace TicketSystem.Web.Models.Project
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }

        public required string Description { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }


        public int WorkflowId { get; set; }

        public bool IsFinished { get; set; }

        public bool IsDeleted { get; set; }

    }
}

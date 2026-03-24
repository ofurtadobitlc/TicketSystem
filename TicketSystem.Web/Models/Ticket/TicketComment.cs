namespace TicketSystem.Web.Models.Ticket
{
    public class TicketComment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public required string CreatorId { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

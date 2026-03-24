namespace TicketSystem.Web.Models.Ticket
{
    public class TicketDependency
    {
        public int BlockedTicketId { get; set; }
        public int BlockingTicketId { get; set; }
    }
}

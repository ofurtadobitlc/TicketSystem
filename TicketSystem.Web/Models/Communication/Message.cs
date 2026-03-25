using TicketSystem.Web.Models.Account;

namespace TicketSystem.Web.Models.Communication
{
    public class Message
    {
        public int Id { get; set; }
        public required string SenderId { get; set; }

        public AppUser? Sender { get; set; }
        public required string ReceiverId { get; set; }
        public AppUser? Receiver { get; set; }

        public required string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}

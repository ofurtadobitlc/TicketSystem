using System.ComponentModel.DataAnnotations.Schema;
using TicketSystem.Web.Models.Account;

namespace TicketSystem.Web.Models.Ticket
{
    public class TicketComment
    {
        public int Id { get; set; }
        [ForeignKey("TicketModel")]
        public int TicketId { get; set; }

        public TicketModel? Ticket { get; set; }

        [ForeignKey("AppUser")]
        public required string CreatorId { get; set; }
        public AppUser? Creator { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using TicketSystem.Web.Models.Account;

namespace TicketSystem.Web.Models.Ticket
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        
        public int TicketId { get; set; }
        public TicketModel? Ticket { get; set; }
        public required string Filename { get; set; }
        [ForeignKey("AppUser")]
        public required string UploadedById { get; set; }

        public AppUser? UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}

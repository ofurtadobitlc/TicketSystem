namespace TicketSystem.Web.Models.Ticket
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public required string Filename { get; set; }
        public required string UploadedById { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}

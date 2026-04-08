namespace TicketSystem.Web.Models.Communication
{
    public class ConversationVM
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageDate { get; set; }
        public int UnreadCount { get; set; }
        public string Initials { get; set; }
        public bool IsOnline { get; set; }
    }

    public class MessageVM
    {
        public string SenderId { get; set; }
        public string Content { get; set; }
        public string SentAt { get; set; }
        public bool IsMine { get; set; }
    }
}

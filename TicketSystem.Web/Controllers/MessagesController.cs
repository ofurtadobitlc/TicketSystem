using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TicketSystem.Web.Models;
using TicketSystem.Web.Models.Account;
using TicketSystem.Web.Models.Communication;

namespace TicketSystem.Web.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(AppDbContext context, UserManager<AppUser> userManager, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Retorna a lista de usuários para iniciar uma nova conversa
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var users = await _userManager.Users
                .Where(u => u.Id != currentUserId)
                .Select(u => new {
                    u.Id,
                    u.Name,
                    Initials = AvatarHelper.GetInitials(u.Name)
                })
                .ToListAsync();
            return Json(users);
        }


        [HttpGet]
        public async Task<IActionResult> GetConversations()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversations = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            var grouped = conversations
                .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                .Select(g => {
                    var otherUser = g.First().SenderId == currentUserId ? g.First().Receiver : g.First().Sender;
                    return new ConversationVM
                    {
                        UserId = otherUser.Id,
                        Name = otherUser.Name,
                        LastMessage = g.First().Content,
                        LastMessageDate = g.First().SentAt,
                        UnreadCount = g.Count(m => m.ReceiverId == currentUserId && !m.IsRead),
                        Initials = AvatarHelper.GetInitials(otherUser.Name),

                        // ADD THIS: Check the Hub to see if they are online right now
                        IsOnline = ChatHub.IsUserOnline(otherUser.Id)
                    };
                }).ToList();

            return Json(grouped);
        }

        // Retorna o histórico de mensagens com um usuário específico
        [HttpGet]
        public async Task<IActionResult> GetChatHistory(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var messages = await _context.Messages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                            (m.SenderId == userId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.SentAt)
                .Select(m => new MessageVM
                {
                    SenderId = m.SenderId,
                    Content = m.Content,
                    SentAt = m.SentAt.ToString("HH:mm"),
                    IsMine = m.SenderId == currentUserId
                })
                .ToListAsync();

            // Marcar como lidas ao abrir a conversa
            var unread = await _context.Messages
                .Where(m => m.SenderId == userId && m.ReceiverId == currentUserId && !m.IsRead)
                .ToListAsync();

            if (unread.Any())
            {
                unread.ForEach(m => m.IsRead = true);
                await _context.SaveChangesAsync();

                // Atualiza o sino de notificações do usuário atual
                var newTotalUnread = await _context.Messages.CountAsync(m => m.ReceiverId == currentUserId && !m.IsRead);
                await _hubContext.Clients.User(currentUserId).SendAsync("ReceiveNotification", newTotalUnread);
            }

            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string receiverId, string content)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var message = new Message
            {
                SenderId = currentUserId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Envia a mensagem em tempo real via SignalR
            await _hubContext.Clients.User(receiverId).SendAsync("ReceiveMessage", currentUserId, content);

            // Notifica o destinatário sobre a nova mensagem (sino)
            var unreadCount = await _context.Messages.CountAsync(m => m.ReceiverId == receiverId && !m.IsRead);
            await _hubContext.Clients.User(receiverId).SendAsync("ReceiveNotification", unreadCount);

            return Ok();
        }
    }
}

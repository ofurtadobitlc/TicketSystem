using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace TicketSystem.Web.Models
{

    [Authorize]
    public class ChatHub : Hub
    {
        // Tracks UserId -> List of ConnectionIds
        private static readonly ConcurrentDictionary<string, HashSet<string>> UserConnections = new();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            var connectionId = Context.ConnectionId;

            if (userId != null)
            {
                var connections = UserConnections.GetOrAdd(userId, _ => new HashSet<string>());

                lock (connections)
                {
                    connections.Add(connectionId);

                    // If this is their FIRST connection, tell everyone they are online
                    if (connections.Count == 1)
                    {
                        Clients.Others.SendAsync("UserPresenceChanged", userId, true);
                    }
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            var connectionId = Context.ConnectionId;

            if (userId != null && UserConnections.TryGetValue(userId, out var connections))
            {
                lock (connections)
                {
                    connections.Remove(connectionId);

                    // If they have NO MORE connections open, tell everyone they are offline
                    if (connections.Count == 0)
                    {
                        UserConnections.TryRemove(userId, out _);
                        Clients.Others.SendAsync("UserPresenceChanged", userId, false);
                    }
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        // Helper to check if a user is currently online
        public static bool IsUserOnline(string userId)
        {
            return UserConnections.TryGetValue(userId, out var connections) && connections.Count > 0;
        }
    }

}

using Microsoft.AspNetCore.SignalR;
using SonoTracker.Api.Hubs;
using SonoTracker.Application.Services.TrackerNotification.Chat;
using SonoTracker.Common.DTO.TrackerNotification.Chat;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Api.Services.TrackerNotification
{
    public class ChatRealtimePublisher(IHubContext<ChatHub> hubContext) : IChatRealtimePublisher
    {
        public Task PublishMessageAsync(
            ChatMessageDto message,
            IReadOnlyList<string> participantUserIds,
            CancellationToken cancellationToken = default)
        {
            var distinctParticipantIds = participantUserIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            if (distinctParticipantIds.Count == 0)
            {
                return Task.CompletedTask;
            }

            var deliveries = distinctParticipantIds.Select(userId =>
                hubContext.Clients
                    .User(userId)
                    .SendAsync("ReceiveMessage", message, cancellationToken));

            return Task.WhenAll(deliveries);
        }

        public Task PublishConversationUpdatedAsync(
            string userId,
            ChatConversationUpdatedDto update,
            CancellationToken cancellationToken = default)
        {
            return hubContext.Clients
                .User(userId)
                .SendAsync("ConversationUpdated", update, cancellationToken);
        }

        public Task PublishChatUnreadCountAsync(
            string userId,
            int unreadCount,
            CancellationToken cancellationToken = default)
        {
            return hubContext.Clients
                .User(userId)
                .SendAsync("ChatUnreadCountUpdated", unreadCount, cancellationToken);
        }

        public Task PublishUserPresenceChangedAsync(
            string userId,
            bool isOnline,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Task.CompletedTask;
            }

            return hubContext.Clients
                .All
                .SendAsync("UserPresenceChanged", userId, isOnline, cancellationToken);
        }
    }
}

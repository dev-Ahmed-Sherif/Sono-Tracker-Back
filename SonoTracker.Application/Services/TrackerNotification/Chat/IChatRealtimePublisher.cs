using SonoTracker.Common.DTO.TrackerNotification.Chat;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.TrackerNotification.Chat
{
    public interface IChatRealtimePublisher
    {
        Task PublishMessageAsync(
            ChatMessageDto message,
            IReadOnlyList<string> participantUserIds,
            CancellationToken cancellationToken = default);

        Task PublishConversationUpdatedAsync(
            string userId,
            ChatConversationUpdatedDto update,
            CancellationToken cancellationToken = default);

        Task PublishChatUnreadCountAsync(
            string userId,
            int unreadCount,
            CancellationToken cancellationToken = default);

        Task PublishUserPresenceChangedAsync(
            string userId,
            bool isOnline,
            CancellationToken cancellationToken = default);
    }
}

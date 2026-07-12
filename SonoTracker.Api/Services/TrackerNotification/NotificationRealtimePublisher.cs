using Microsoft.AspNetCore.SignalR;
using SonoTracker.Api.Hubs;
using SonoTracker.Application.Services.TrackerNotification.Notification;
using SonoTracker.Common.DTO.TrackerNotification.Notification;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Api.Services.TrackerNotification
{
    public class NotificationRealtimePublisher(IHubContext<NotificationHub> hubContext)
        : INotificationRealtimePublisher
    {
        public Task PublishNotificationAsync(
            string userId,
            NotificationDto notification,
            CancellationToken cancellationToken = default)
        {
            return hubContext.Clients
                .User(userId)
                .SendAsync("ReceiveNotification", notification, cancellationToken);
        }

        public Task PublishUnreadCountAsync(
            string userId,
            int unreadCount,
            CancellationToken cancellationToken = default)
        {
            return hubContext.Clients
                .User(userId)
                .SendAsync("UnreadCountUpdated", unreadCount, cancellationToken);
        }
    }
}

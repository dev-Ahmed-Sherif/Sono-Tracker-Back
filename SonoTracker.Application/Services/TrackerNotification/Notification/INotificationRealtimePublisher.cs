using SonoTracker.Common.DTO.TrackerNotification.Notification;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.TrackerNotification.Notification
{
    public interface INotificationRealtimePublisher
    {
        Task PublishNotificationAsync(
            string userId,
            NotificationDto notification,
            CancellationToken cancellationToken = default);

        Task PublishUnreadCountAsync(
            string userId,
            int unreadCount,
            CancellationToken cancellationToken = default);
    }
}

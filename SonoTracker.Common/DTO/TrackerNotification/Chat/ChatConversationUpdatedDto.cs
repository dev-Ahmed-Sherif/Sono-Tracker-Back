using System;

namespace SonoTracker.Common.DTO.TrackerNotification.Chat
{
    public class ChatConversationUpdatedDto
    {
        public required string Id { get; set; }
        public string? LastMessagePreview { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UnreadCount { get; set; }
    }
}

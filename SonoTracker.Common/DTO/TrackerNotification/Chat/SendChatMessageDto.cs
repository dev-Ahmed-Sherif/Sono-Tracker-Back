using System.ComponentModel.DataAnnotations;

namespace SonoTracker.Common.DTO.TrackerNotification.Chat
{
    public class SendChatMessageDto
    {
        [Required]
        public required string ConversationId { get; set; }

        [Required, MinLength(1)]
        public required string Content { get; set; }
    }
}

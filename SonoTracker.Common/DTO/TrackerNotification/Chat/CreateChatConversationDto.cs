using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SonoTracker.Common.DTO.TrackerNotification.Chat
{
    public class CreateChatConversationDto
    {
        [Required, MinLength(1)]
        public required List<string> ParticipantUserIds { get; set; }
    }
}

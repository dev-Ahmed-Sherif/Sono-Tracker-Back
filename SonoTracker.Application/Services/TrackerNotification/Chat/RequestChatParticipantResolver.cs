using SonoTracker.Common.Constants.TrackerNotification;
using SonoTracker.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SonoTracker.Application.Services.TrackerNotification.Chat
{
    internal sealed record RequestChatContext(
        string RequestId,
        string RequestNumber,
        string OwnerUserId,
        string? LeaderUserId,
        string? ReceptionUserId);

    /// <summary>
    /// Request-scoped chat participants. Tracker has no housing request entity yet;
    /// endpoints remain API-compatible and return not-found until wired to a domain request.
    /// </summary>
    internal sealed class RequestChatParticipantResolver(SonoTrackerDbContext context)
    {
        public Task<RequestChatContext?> LoadContextAsync(
            string requestId,
            CancellationToken cancellationToken = default)
        {
            _ = requestId;
            _ = cancellationToken;
            return Task.FromResult<RequestChatContext?>(null);
        }

        public IReadOnlyList<string> GetParticipantsForGroupType(RequestChatContext chatContext, string groupType)
        {
            _ = chatContext;
            if (!RequestChatGroupTypes.IsValid(groupType))
            {
                return Array.Empty<string>();
            }

            return Array.Empty<string>();
        }

        public IReadOnlyList<string> GetAvailableGroupTypesForUser(RequestChatContext chatContext, string userId)
        {
            _ = chatContext;
            _ = userId;
            return Array.Empty<string>();
        }
    }
}

using SonoTracker.Common.Constants.Auth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SonoTracker.Application.Services.TrackerNotification.Chat
{
    /// <summary>
    /// Role-based rules for who may appear in chat contact lists and direct conversations.
    /// </summary>
    internal static class ChatContactVisibilityResolver
    {
        /// <summary>
        /// User → Leader, ReceptionStaff
        /// Leader → all ReceptionStaff; User contacts only from direct 1:1 history (no groups)
        /// ReceptionStaff → all Leaders; User contacts only from direct 1:1 history (no groups)
        /// Admin / SuperAdmin → User, Leader, ReceptionStaff
        /// </summary>
        private static readonly IReadOnlyDictionary<string, HashSet<string>> AllowedTargetRolesByRole =
            new Dictionary<string, HashSet<string>>(StringComparer.Ordinal)
            {
                [Roles.User] = new HashSet<string>(StringComparer.Ordinal)
                {
                    Roles.Leader,
                    Roles.ReceptionStaff
                },
                [Roles.Leader] = new HashSet<string>(StringComparer.Ordinal)
                {
                    Roles.ReceptionStaff
                },
                [Roles.ReceptionStaff] = new HashSet<string>(StringComparer.Ordinal)
                {
                    Roles.Leader
                },
                [Roles.Admin] = new HashSet<string>(StringComparer.Ordinal)
                {
                    Roles.User,
                    Roles.Leader,
                    Roles.ReceptionStaff
                },
                [Roles.SuperAdmin] = new HashSet<string>(StringComparer.Ordinal)
                {
                    Roles.User,
                    Roles.Leader,
                    Roles.ReceptionStaff
                }
            };

        public static IReadOnlyCollection<string> GetAllowedTargetRoles(string? currentUserRole)
        {
            if (string.IsNullOrWhiteSpace(currentUserRole) ||
                !AllowedTargetRolesByRole.TryGetValue(currentUserRole.Trim(), out var roles))
            {
                return Array.Empty<string>();
            }

            return roles;
        }

        public static bool AllowsHistoryPartners(string? currentUserRole) =>
            string.Equals(currentUserRole?.Trim(), Roles.Leader, StringComparison.Ordinal) ||
            string.Equals(currentUserRole?.Trim(), Roles.ReceptionStaff, StringComparison.Ordinal);

        public static bool RestrictsHistoryToDirectUserContacts(string? currentUserRole)
        {
            var role = currentUserRole?.Trim();
            return string.Equals(role, Roles.Leader, StringComparison.Ordinal) ||
                   string.Equals(role, Roles.ReceptionStaff, StringComparison.Ordinal);
        }

        public static bool ExcludesGroupConversations(string? currentUserRole) =>
            RestrictsHistoryToDirectUserContacts(currentUserRole);

        public static bool CanChatWith(string? currentUserRole, string? targetUserRole)
        {
            if (string.IsNullOrWhiteSpace(currentUserRole) || string.IsNullOrWhiteSpace(targetUserRole))
            {
                return false;
            }

            return GetAllowedTargetRoles(currentUserRole)
                .Contains(targetUserRole.Trim(), StringComparer.Ordinal);
        }

        public static bool CanChatWith(
            string? currentUserRole,
            string? targetUserRole,
            bool isHistoryPartner)
        {
            if (isHistoryPartner && AllowsHistoryPartners(currentUserRole))
            {
                if (RestrictsHistoryToDirectUserContacts(currentUserRole))
                {
                    return string.Equals(targetUserRole?.Trim(), Roles.User, StringComparison.Ordinal);
                }

                return true;
            }

            return CanChatWith(currentUserRole, targetUserRole);
        }
    }
}

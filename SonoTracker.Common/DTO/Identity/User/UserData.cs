using System.Collections.Generic;

namespace SonoTracker.Common.DTO.Identity.User
{
    public record UserData(string Id, 
                           string Name, 
                           string Role, 
                           List<UserPermission> Permissions, 
                           string OrganizationId,
                           string FloatingUnitId);
}

using Microsoft.AspNetCore.Authorization;

namespace api.Models
{
    public class PermissionRequirement(Helpers.Enums.Permission[] permissionList) : IAuthorizationRequirement
    {
        public Helpers.Enums.Permission[] PermissionList { get; set; } = permissionList;
    }
}
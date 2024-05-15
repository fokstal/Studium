using api.Helpers.Enums;
using api.Services.DataServices;

namespace api.Services
{
    public class PermissionService(UserService userService)
    {
        public HashSet<PermissionEnum> GetPermissionList(int userId)
        {
            return userService.GetPermissionList(userId);
        }
    }
}
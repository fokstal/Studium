using api.Services.DataServices;

namespace api.Services
{
    public class PermissionService(UserService userService)
    {
        public HashSet<Helpers.Enums.Permission> GetPermissionList(int userId)
        {
            return userService.GetPermissionList(userId);
        }
    }
}
using api.Helpers.Enums;
using api.Repositories.Data;

namespace api.Services
{
    public class PermissionService(UserRepository userService)
    {
        public HashSet<PermissionEnum> GetPermissionList(int userId)
        {
            return userService.GetPermissionList(userId);
        }
    }
}
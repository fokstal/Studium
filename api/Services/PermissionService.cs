using api.Helpers.Enums;
using api.Repositories.Data;

namespace api.Services
{
    public class PermissionService(UserRepository userService)
    {
        public async Task<HashSet<PermissionEnum>> GetPermissionListAsync(int userId)
        {
            return await userService.GetPermissionListAsync(userId);
        }
    }
}
using api.Models;
using api.Services.DataServices;

namespace api.Services
{
    public class PermissionService(UserService userService) : IPermissionService
    {
        public Task<HashSet<Helpers.Enums.Permission>> GetPermissionListAsync(int userId)
        {
            return userService.GetPermissionListAsync(userId);
        }
    }
}
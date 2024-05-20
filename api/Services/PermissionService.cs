using api.Helpers.Enums;
using api.Repositories.Data;

namespace api.Services
{
    public class PermissionService(UserRepository userRepository)
    {
        public async Task<HashSet<PermissionEnum>> GetPermissionListAsync(int userId)
        {
            return await userRepository.GetPermissionListAsync(userId);
        }
    }
}
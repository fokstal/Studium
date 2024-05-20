using api.Helpers.Enums;
using api.Repositories.Data;

namespace api.Services
{
    public class RoleService(UserRepository userRepository)
    {
        public async Task<HashSet<RoleEnum>> GetRoleListAsync(int userId)
        {
            return await userRepository.GetRoleListAsync(userId);
        }
    }
}
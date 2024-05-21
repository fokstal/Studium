using api.Helpers.Enums;
using api.Models;
using api.Repositories.Data;

namespace api.Services
{
    public class RoleService(UserRepository userRepository)
    {
        public async Task<HashSet<RoleEnum>> GetRoleListAsync(Guid userId)
        {
            return await userRepository.GetRoleListAsync(userId);
        }
    }
}
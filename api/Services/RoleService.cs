using api.Helpers.Enums;
using api.Repositories.Data;

namespace api.Services
{
    public class RoleService(UserRepository userRepository)
    {
        public HashSet<RoleEnum> GetRoleList(int userId)
        {
            return userRepository.GetRoleList(userId);
        }
    }
}
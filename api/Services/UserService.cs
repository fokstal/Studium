using api.Helpers.Enums;
using api.Models;
using api.Repositories.Data;

namespace api.Services
{
    public static class UserService
    {
        public static bool CheckRoleContains(UserRepository userRepository, UserEntity user, RoleEnum role)
        {
            HashSet<RoleEnum> roleList = userRepository.GetRoleListAsync(user.Id).Result;

            user.RoleList = roleList.ToList().Select
            (
                role => new RoleEntity()
                {
                    Id = Convert.ToInt32(role),
                    Name = role.ToString(),
                }
            )
            .ToList();

            return user.RoleList.Select(roleDb => roleDb.Name).ToList().Contains(role.ToString());
        }
    }
}
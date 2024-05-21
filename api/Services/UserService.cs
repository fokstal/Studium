using api.Helpers.Enums;
using api.Models;

namespace api.Services
{
    public static class UserService
    {
        public static bool CheckRoleContains(UserEntity user, RoleEnum role)
        {
            return user.RoleList.Select(roleDb => roleDb.Name).ToList().Contains(role.ToString());
        }
    }
}
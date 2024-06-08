using api.Helpers.Enums;
using api.Models;
using api.Repositories.Data;

using static api.Helpers.Enums.RoleEnum;

namespace api.Services
{
    public class Authorizing(UserRepository userRepository, HttpContext httpContext)
    {
        public async Task<bool> RequireOwnerListAccess(OwnerParameters[] ownerList)
        {
            bool access = true;

            foreach (OwnerParameters owner in ownerList)
            {
                access = await RequireOwnerAccess(owner);
            }

            return access;
        }

        public async Task<bool> RequireOwnerAccess(OwnerParameters owner)
        {
            Guid userIdFromCookie = new HttpContextService(httpContext).GetUserIdFromCookie();

            RoleService roleService = httpContext.RequestServices.GetRequiredService<RoleService>();

            HashSet<RoleEnum> roleList = roleService.GetRoleListAsync(userIdFromCookie).Result;

            if (roleList.Intersect([Admin, Secretar]).Any()) return true;
            if (!roleList.Contains(owner.Role)) return false;

            UserEntity user = await userRepository.GetAsync(Convert.ToInt32(userIdFromCookie)) ?? throw new ArgumentNullException("User is null!");
            
            if (!owner.IdList.ToList().Contains(user.Id)) return false;

            return true;
        }
    }
}
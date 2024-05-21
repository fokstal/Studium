using api.Helpers.Enums;
using api.Models;
using api.Repositories.Data;
using Microsoft.AspNetCore.Mvc;

namespace api.Services
{
    public class PermissionService(UserRepository userRepository)
    {
        public async Task<HashSet<PermissionEnum>> GetPermissionListAsync(Guid userId)
        {
            return await userRepository.GetPermissionListAsync(userId);
        }

        public async Task<ActionResult> RequireUserAccess(HttpContext httpContext, Guid[] requiredIdList, RoleEnum requireRole)
        {
            Guid userIdFromCookie = new HttpContextService(httpContext).GetUserIdFromCookie();

            RoleService roleService = httpContext.RequestServices.GetRequiredService<RoleService>();

            HashSet<RoleEnum> roleList = roleService.GetRoleListAsync(userIdFromCookie).Result;

            if (roleList.Intersect([RoleEnum.Admin, RoleEnum.Secretar]).Any()) return new OkResult();
            if (!roleList.Contains(requireRole)) return new ForbidResult();

            UserEntity user = await userRepository.GetAsync(Convert.ToInt32(userIdFromCookie)) ?? throw new ArgumentNullException("User is null!");
            
            if (!requiredIdList.ToList().Contains(user.Id)) return new ForbidResult();

            return new OkResult();
        }
    }
}
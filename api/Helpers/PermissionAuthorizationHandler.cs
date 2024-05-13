using System.Security.Claims;
using api.Helpers.Constants;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;

namespace api.Helpers
{
    public class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            Claim? userIdClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.UserId);

            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int id))
            {
                return;
            }

            using IServiceScope scope = serviceScopeFactory.CreateScope();

            IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

            HashSet<Enums.Permission> permissionList = await permissionService.GetPermissionListAsync(id);

            if (permissionList.Intersect(requirement.PermissionList).Any())
            {
                context.Succeed(requirement);
            } 
        }
    }
}
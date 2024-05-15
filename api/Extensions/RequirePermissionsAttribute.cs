using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Helpers.Constants;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Extensions
{
    public class RequirePermissionsAttribute(Helpers.Enums.Permission[] permissionListRequirement) : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string? token = context.HttpContext.Request.Cookies["Cookie"];
            JwtSecurityTokenHandler handler = new();

            JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

            Claim userIdClaim = jwtToken.Claims.First(claim => claim.Type == CustomClaims.UserId);

            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return;
            }

            PermissionService permissionService = context.HttpContext.RequestServices.GetRequiredService<PermissionService>();

            HashSet<Helpers.Enums.Permission> permissionList = permissionService.GetPermissionList(userId);

            if (!permissionList.Intersect(permissionListRequirement).Any())
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
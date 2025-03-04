using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Helpers.Constants;
using api.Helpers.Enums;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Extensions
{
    public class RequireRolesAttribute(RoleEnum[] roleListRequirement) : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string? token = context.HttpContext.Request.Cookies[CookieNames.USER_TOKEN];
            JwtSecurityTokenHandler handler = new();

            JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

            Claim userIdClaim = jwtToken.Claims.First(claim => claim.Type == CustomClaims.USER_ID);

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return;
            }

            RoleService roleService = context.HttpContext.RequestServices.GetRequiredService<RoleService>();

            HashSet<RoleEnum> roleList = roleService.GetRoleListAsync(userId).Result;

            if (!roleList.Intersect(roleListRequirement).Any())
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
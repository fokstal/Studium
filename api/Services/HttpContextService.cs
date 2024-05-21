using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Helpers.Constants;
using api.Models;
using api.Repositories.Data;

namespace api.Services
{
    public class HttpContextService(HttpContext httpContext)
    {
        public Guid GetUserIdFromCookie()
        {
            string? token = httpContext.Request.Cookies[CookieNames.USER_TOKEN];
            JwtSecurityTokenHandler handler = new();

            JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

            Claim userIdClaim = jwtToken.Claims.First(claim => claim.Type == CustomClaims.USER_ID);

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new Exception("User is not authorize!");
            }
            
            return userId;
        }

        public async Task<UserEntity> GetUserFromCookie(UserRepository userRepository)
        {
            Guid userIdFromCookie = new HttpContextService(httpContext).GetUserIdFromCookie();

            return await userRepository.GetAsync(Convert.ToInt32(userIdFromCookie)) ?? throw new ArgumentNullException("User is null!");
        }
    }
}
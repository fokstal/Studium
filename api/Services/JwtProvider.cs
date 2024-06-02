using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class JwtProvider(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateToken(UserEntity user)
        {
            string? jwtKey = _configuration["Jwt:Key"] ?? throw new NullReferenceException(nameof(jwtKey));
            string? jwtExpiresSeconds = _configuration["Jwt:ExpiresSeconds"] ?? throw new NullReferenceException(nameof(jwtExpiresSeconds));

            JwtSecurityToken token = new
            (
                claims: [new("userId", user.Id.ToString())],
                expires: DateTime.Now.Add(TimeSpan.FromSeconds(Convert.ToInt32(jwtExpiresSeconds))),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    SecurityAlgorithms.HmacSha256
                )
            );

            string encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;
        }

        public static Claim GetClaimFromToken(string token, string claimName)
        {
            JwtSecurityTokenHandler handler = new();

            JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

            return jwtToken.Claims.First(claim => claim.Type == claimName);
        }
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using api.Model;
using Microsoft.IdentityModel.Tokens;

namespace api.Service
{
    public class JwtProvider(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateToken(User user)
        {
            string? jwtKey = _configuration["Jwt:Key"];
            string? jwtExpiresSeconds = _configuration["Jwt:ExpiresSeconds"];

            if (jwtKey is null) throw new NullReferenceException(nameof(jwtKey));
            if (jwtExpiresSeconds is null) throw new NullReferenceException(nameof(jwtExpiresSeconds));

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
    }
}
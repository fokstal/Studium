using System.IdentityModel.Tokens.Jwt;
using System.Text;
using api.Model;
using Microsoft.IdentityModel.Tokens;

namespace api.Service
{
    public class JwtProvider(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;
        private static readonly string defaultKey = "secretKeysecretKeysecretKeysecretKeysecretKeysecretKeysecretKeysecretKey";

        public static string DefaultKey { get => defaultKey; }

        public string GenerateToken(User user)
        {
            JwtSecurityToken token = new
            (
                claims: [new("userId", user.Id.ToString())],
                expires: DateTime.Now.Add(TimeSpan.FromSeconds(Convert.ToInt32(_configuration["Jwt:ExpiresSeconds"]))),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? defaultKey)), 
                    SecurityAlgorithms.HmacSha256
                )
            );

            string encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;
        }
    }
}
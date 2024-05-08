using System.IdentityModel.Tokens.Jwt;
using System.Text;
using api.Model;
using Microsoft.IdentityModel.Tokens;

namespace api.Service
{
    public static class JwtProvider
    {
        private static readonly string secretKey = "secretKeysecretKeysecretKeysecretKeysecretKeysecretKeysecretKeysecretKey";
        private static readonly int expiresSecondsJWT = 3600;

        public static string SecretKey { get => secretKey; }

        public static string GenerateToken(User user)
        {
            JwtSecurityToken token = new
            (
                claims: [new("userId", user.Id.ToString())],
                expires: DateTime.Now.Add(TimeSpan.FromSeconds(expiresSecondsJWT)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), 
                    SecurityAlgorithms.HmacSha256
                )
            );

            string encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;
        }
    }
}
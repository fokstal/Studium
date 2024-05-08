using System.Text;
using api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace api.Extensions
{
    public static class AppExtensions
    {
        public static void AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.TokenValidationParameters = new()
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtProvider.SecretKey)),
                        };

                        options.Events = new()
                        {
                            OnMessageReceived = context => 
                            {
                                context.Token = context.Request.Cookies["Cookie"];

                                return Task.CompletedTask;
                            }
                        };
                    });

            services.AddAuthorization();
        }
    }
}
using System.Text;
using api.Services;
using api.Services.DataServices;
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
                        string? jwtKey = configuration["Jwt:Key"] ?? throw new NullReferenceException(nameof(jwtKey));

                        options.TokenValidationParameters = new()
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
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

            services.AddScoped<PermissionService>();
            services.AddScoped<UserService>();

            services.AddAuthorization();
        }
    }
}
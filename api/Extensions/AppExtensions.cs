using System.Text;
using api.Services;
using api.Repositories.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using api.Helpers.Constants;

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
                                context.Token = context.Request.Cookies[CookieNames.USER_TOKEN];

                                return Task.CompletedTask;
                            }
                        };
                    });
        }

        public static void AddAppAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<PermissionService>();
            services.AddScoped<RoleService>();

            services.AddScoped<UserRepository>();

            services.AddAuthorization();
        }
    }
}
using System.Text;
using api.Helpers;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

            services.AddScoped<IPermissionService, PermissionService>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddAuthorization();
        }

        public static IEndpointConventionBuilder RequirePermissions<TBuilder>
            (this TBuilder builder, params Helpers.Enums.Permission[] permissionList)
            where TBuilder : IEndpointConventionBuilder
        {
            return builder.RequireAuthorization
            (
                policy => policy.AddRequirements(new PermissionRequirement(permissionList))
            );
        }
    }
}
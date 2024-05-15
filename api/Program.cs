using api.Data;
using api.Extensions;
using api.Models;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
IServiceCollection services = builder.Services;

string? corsName = configuration["Cors:Name"] ?? throw new NullReferenceException(nameof(corsName));
string? connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? throw new NullReferenceException(nameof(connectionString));

// Add services to the container.

services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

services.AddCors(options => options.AddPolicy(corsName, policy =>
{
    policy
    .WithOrigins("http//localhost:3000")
    .WithHeaders("Content-Type")
    .WithMethods("PUT", "DELETE")
    .AllowCredentials();
}));

services.AddAppAuthentication(builder.Configuration);

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

WebApplication app = builder.Build();

app.UseCors(corsName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new()
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();

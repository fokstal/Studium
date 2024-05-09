using api.Data;
using api.Extensions;
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
    .AllowAnyOrigin()
    .WithHeaders("Content-Type");
}));

services.AddAppAuthentication(builder.Configuration);

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

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

app.Run();

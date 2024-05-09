using api.Data;
using api.Extensions;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

string corsName = configuration["Cors:Name"] ?? "DefaultCorsPolicy";
string connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=AppData/Database.db";

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>
(
    options => options.UseSqlite(connectionString)
);

builder.Services.AddCors(options => options.AddPolicy(corsName,
    policy =>
    {
        policy
        .AllowAnyOrigin()
        .WithHeaders("Content-Type");
    }));

builder.Services.AddAppAuthentication(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   

var app = builder.Build();

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

using api.Configurations;
using api.Model;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace api.Data
{
    public class AppDbContext : DbContext
    {
        private readonly AuthorizationOptions _authOptions;

        public DbSet<Grade> Grade { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Passport> Passport { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Subject> Subject { get; set; }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set;}
        public DbSet<UserRole> UserRole { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<AuthorizationOptions> authOptions) : base(options)
        {
            _authOptions = authOptions.Value;

            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(_authOptions));
        }
    }
}
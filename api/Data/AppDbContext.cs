using api.Data.Configurations;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace api.Data
{
    public class AppDbContext : DbContext
    {
        private readonly AuthorizationOptions _authOptions;

        public DbSet<GradeEntity> Grade { get; set; }
        public DbSet<GroupEntity> Group { get; set; }
        public DbSet<PassportEntity> Passport { get; set; }
        public DbSet<PersonEntity> Person { get; set; }
        public DbSet<StudentEntity> Student { get; set; }
        public DbSet<SubjectEntity> Subject { get; set; }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<RoleEntity> Role { get; set;}

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
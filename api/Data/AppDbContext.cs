using api.Model;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Grade> Grade { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Passport> Passport { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Subject> Subject { get; set; }

        public DbSet<User> User { get; set; }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=AppData/Database.db");
        }
    }
}
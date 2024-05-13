using api.Model;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);

            builder.HasMany(user => user.RoleList)
                .WithMany(role => role.UserList)
                .UsingEntity<UserRole>
                (
                    l => l.HasOne<Role>().WithMany().HasForeignKey(l => l.RoleId),
                    r => r.HasOne<User>().WithMany().HasForeignKey(r => r.UserId)
                );
        }
    }
}
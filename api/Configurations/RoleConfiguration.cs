using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(role => role.Id);

            builder.HasMany(role => role.PermissionList)
                .WithMany(permission => permission.RoleList)
                .UsingEntity<RolePermission>
                (
                    r => r.HasOne<Permission>().WithMany().HasForeignKey(r => r.PermissionId),
                    l => l.HasOne<Role>().WithMany().HasForeignKey(l => l.RoleId)
                );

            IEnumerable<Role> roleList = Enum.GetValues<Helpers.Enums.Role>().Select(roleEnum => new Role
                {
                    Id = Convert.ToInt32(roleEnum),
                    Name = roleEnum.ToString(),
                });

            builder.HasData(roleList);
        }
    }
}
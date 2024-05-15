using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(role => role.Id);

            builder.HasMany(role => role.PermissionList)
                .WithMany(permission => permission.RoleList)
                .UsingEntity<RolePermissionEntity>
                (
                    r => r.HasOne<PermissionEntity>().WithMany().HasForeignKey(r => r.PermissionId),
                    l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(l => l.RoleId)
                );

            IEnumerable<RoleEntity> roleList = Enum.GetValues<Helpers.Enums.Role>().Select(roleEnum => new RoleEntity
                {
                    Id = Convert.ToInt32(roleEnum),
                    Name = roleEnum.ToString(),
                });

            builder.HasData(roleList);
        }
    }
}
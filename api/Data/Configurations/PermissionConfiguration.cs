using api.Helpers.Enums;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.HasKey(permission => permission.Id);

            IEnumerable<PermissionEntity> permissionList = Enum.GetValues<PermissionEnum>().Select(permissionEnum => new PermissionEntity
                {
                    Id = Convert.ToInt32(permissionEnum),
                    Name = permissionEnum.ToString(),
                });

            builder.HasData(permissionList);
        }
    }
}
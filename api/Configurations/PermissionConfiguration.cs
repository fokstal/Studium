using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(permission => permission.Id);

            IEnumerable<Permission> permissionList = Enum.GetValues<Helpers.Enums.Permission>().Select(permissionEnum => new Permission
                {
                    Id = Convert.ToInt32(permissionEnum),
                    Name = permissionEnum.ToString(),
                });

            builder.HasData(permissionList);
        }
    }
}
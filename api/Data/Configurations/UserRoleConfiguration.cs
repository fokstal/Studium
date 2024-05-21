using api.Helpers.Enums;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
        {
            builder.HasKey(userRole => new { userRole.UserId, userRole.RoleId });

            IEnumerable<UserRoleEntity> userRoleList = Enum.GetValues<RoleEnum>().Select(roleEnum => new UserRoleEntity
            {
                UserId = Convert.ToInt32(roleEnum),
                RoleId = Convert.ToInt32(roleEnum),
            });

            builder.HasData(userRoleList);
        }
    }
}
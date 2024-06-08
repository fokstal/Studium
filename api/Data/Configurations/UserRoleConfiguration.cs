using api.Helpers.Enums;
using api.Models;
using api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
        {
            builder.HasKey(userRole => new { userRole.UserEntityId, userRole.RoleEntityId });

            IEnumerable<UserRoleEntity> userRoleList = Enum.GetValues<RoleEnum>().Select(roleEnum => new UserRoleEntity
            {
                UserEntityId = StringHasher.GenerateGuid(Convert.ToInt32(roleEnum).ToString()),
                RoleEntityId = Convert.ToInt32(roleEnum),
            });

            builder.HasData(userRoleList);
        }
    }
}
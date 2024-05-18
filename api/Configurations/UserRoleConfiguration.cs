using api.Helpers.Enums;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(userRole => new { userRole.UserId, userRole.RoleId });

            IEnumerable<UserRole> userRoleList = Enum.GetValues<RoleEnum>().Select(roleEnum => new UserRole
            {
                UserId = Convert.ToInt32(roleEnum),
                RoleId = Convert.ToInt32(roleEnum),
            });

            builder.HasData(userRoleList);
        }
    }
}
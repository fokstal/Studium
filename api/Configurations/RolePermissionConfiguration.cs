using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Configurations
{
    public class RolePermissionConfiguration(AuthorizationOptions authorization) : IEntityTypeConfiguration<RolePermission>
    {
        private readonly AuthorizationOptions _authOptions = authorization;

        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(rolePermission => new { rolePermission.RoleId, rolePermission.PermissionId });

            builder.HasData(ParseRolePermissionList());
        }
        
        private RolePermission[] ParseRolePermissionList()
        {
            return _authOptions.RolePermissionsList.SelectMany
            (
                rolePermission => 
                    rolePermission.PermissionList.Select(permission => new RolePermission
                    {
                        RoleId = Convert.ToInt32(Enum.Parse<Helpers.Enums.Role>(rolePermission.Role)),
                        PermissionId = Convert.ToInt32(Enum.Parse<Helpers.Enums.Permission>(permission)),
                    })
            ).ToArray();
        }
    }
}
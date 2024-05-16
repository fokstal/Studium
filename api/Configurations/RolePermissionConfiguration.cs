using api.Helpers.Enums;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Configurations
{
    public class RolePermissionConfiguration(AuthorizationOptions authorization) : IEntityTypeConfiguration<RolePermissionEntity>
    {
        private readonly AuthorizationOptions _authOptions = authorization;

        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.HasKey(rolePermission => new { rolePermission.RoleId, rolePermission.PermissionId });

            builder.HasData(ParseRolePermissionList());
        }
        
        private RolePermissionEntity[] ParseRolePermissionList()
        {
            return _authOptions.RolePermissionsList.SelectMany
            (
                rolePermission => 
                    rolePermission.PermissionList.Select(permission => new RolePermissionEntity
                    {
                        RoleId = Convert.ToInt32(Enum.Parse<RoleEnum>(rolePermission.Role)),
                        PermissionId = Convert.ToInt32(Enum.Parse<PermissionEnum>(permission)),
                    })
            ).ToArray();
        }
    }
}
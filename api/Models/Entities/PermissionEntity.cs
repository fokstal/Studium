namespace api.Models
{
    public class PermissionEntity : ModelEntity
    {
        public string Name { get; set; } = null!;

        public List<RoleEntity> RoleList { get; set; } = [];
    }
}
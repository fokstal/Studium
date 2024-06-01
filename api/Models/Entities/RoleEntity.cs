namespace api.Models
{
    public class RoleEntity : ModelEntity
    {
        public string Name { get; set; } = null!;

        public List<UserEntity> UserEntityList { get; set; } = [];
        public List<PermissionEntity> PermissionEntityList { get; set; } = [];
    }
}
namespace api.Models
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<UserEntity> UserList { get; set; } = [];
        public List<PermissionEntity> PermissionList { get; set; } = [];
    }
}
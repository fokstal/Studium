namespace api.Models
{
    public class PermissionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<RoleEntity> RoleList { get; set; } = [];
    }
}
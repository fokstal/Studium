namespace api.Models
{
    public class UserEntity : ModelEntity
    {
        public string Login { get; set; } = null!;
        public string? Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        public DateTime DateCreated { get; set; }

        public List<RoleEntity> RoleList { get; set; } = [];
    }
}
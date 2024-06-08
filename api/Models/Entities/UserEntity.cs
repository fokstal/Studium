namespace api.Models
{
    public class UserEntity : ModelEntity
    {
        public new Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime DateCreated { get; set; }

        public List<RoleEntity> RoleList { get; set; } = [];
    }
}
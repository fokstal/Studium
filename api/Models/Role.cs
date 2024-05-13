using api.Model;

namespace api.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<User> UserList { get; set; } = [];
        public List<Permission> PermissionList { get; set; } = [];
    }
}
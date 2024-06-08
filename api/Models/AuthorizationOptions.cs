namespace api.Models
{
    public class AuthorizationOptions
    {
        public RolePermissionList[] RolePermissionsList { get; set; } = [];
    }

    public record class RolePermissionList
    {
        public string Role { get; set; } = null!;

        public string[] PermissionList { get; set; } = [];
    }
}
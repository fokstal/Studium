using api.Helpers.Enums;

namespace api.Models
{
    public class OwnerParameters
    {
        public Guid[] IdList { get; set; } = null!;
        public RoleEnum Role { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using api.Models;

namespace api.Model.DTO
{
    public class RegisterUserDTO
    {
        [Required]
        [Length(3, 10)]
        public string Login { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        [Required]
        [Length(10, 15)]
        public string Password { get; set; } = null!;

        [Required]
        public List<RoleEntity> RoleList { get; set; } = [];
    }
}
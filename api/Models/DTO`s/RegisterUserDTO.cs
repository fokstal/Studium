using System.ComponentModel.DataAnnotations;
using api.Models;

namespace api.Model.DTO
{
    public class RegisterUserDTO
    {
        public Guid Id { get; set;}

        [Required]
        [Length(3, 10)]
        public string Login { get; set; } = null!;

        [Required]
        [Length(3, 15)]
        public string FirstName { get; set; } = null!;

        [Required]
        [Length(3, 25)]
        public string MiddleName { get; set; } = null!;

        [Required]
        [Length(3, 20)]
        public string LastName { get; set; } = null!;


        [Required]
        [Length(10, 15)]
        public string Password { get; set; } = null!;

        [Required]
        public List<RoleEntity> RoleList { get; set; } = [];
    }
}
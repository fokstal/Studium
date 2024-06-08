using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class LoginUserDTO
    {
        [Required]
        [Length(3, 10)]
        public string Login { get; set; } = null!;

        [Required]
        [Length(10, 20)]
        public string Password { get; set; } = null!;
    }
}
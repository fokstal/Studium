using api.Models;

namespace api.Model
{
    public class User : IModel
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string? Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        public DateTime DateCreated { get; set; }
    }
}
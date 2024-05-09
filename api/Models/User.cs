namespace api.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string? Email { get; set; }
        public string PasswordHash { get; set; } = null!;
        public DateTime DateCreated { get; set; }
    }
}
namespace api.Models
{
    public class PersonEntity : IModelEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; } = new();
        public int Sex { get; set; }
        public string AvatarFileName { get; set; } = null!;

        public PassportEntity? Passport { get; set; }
        public StudentEntity? Student { get; set; }
    }
}
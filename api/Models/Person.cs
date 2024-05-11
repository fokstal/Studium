namespace api.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; } = new();
        public int Sex { get; set; }
        public string AvatarFileName { get; set; } = null!;

        public Passport? Passport { get; set; }
        public Student? Student { get; set; }
    }
}
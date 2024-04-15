using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; } = new();

        public int? PassportId { get; set; }
        [ForeignKey(nameof(PassportId))]
        public Passport? Passport { get; set; }
    }
}
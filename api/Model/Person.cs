using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model
{
    public class Person
    {
        public int Id { get; set; }

        [MaxLength(15)]
        public string FirstName { get; set; } = "~имя~";

        [MaxLength(25)]
        public string MiddleName { get; set; } = "~отчество~";
        
        [MaxLength(20)]
        public string LastName { get; set; } = "~фамилия~";
        public DateTime BirthDate { get; set; } = new();

        public int PassportId { get; set; }
        [ForeignKey(nameof(PassportId))]
        public Passport? Passport { get; set; }
    }
}
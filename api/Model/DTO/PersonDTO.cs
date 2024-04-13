using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class PersonDTO
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
    }
}
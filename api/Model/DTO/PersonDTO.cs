using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class PersonDTO
    {
        public int Id { get; set; }

        [Length(3, 15)]
        public string FirstName { get; set; } = "~имя~";

        [Length(3, 25)]
        public string MiddleName { get; set; } = "~отчество~";
        
        [Length(3, 20)]
        public string LastName { get; set; } = "~фамилия~";
        public DateTime BirthDate { get; set; } = new();

        [Range(0, 1)]
        public int Sex { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
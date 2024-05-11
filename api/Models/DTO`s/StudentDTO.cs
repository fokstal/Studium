using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class StudentDTO
    {
        public int Id { get; set; }

        [Required]
        public int PersonId { get; set; }
        public int? GroupId { get; set; }
    }
}
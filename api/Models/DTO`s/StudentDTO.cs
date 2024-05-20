using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class StudentDTO
    {
        [Required]
        public int PersonId { get; set; }
        public int? GroupId { get; set; }
    }
}
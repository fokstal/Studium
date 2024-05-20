using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class SubjectDTO
    {
        [Required]
        [MaxLength(65)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Descripton { get; set; } = null!;

        public int? TeacherId { get; set; }
        public int? GroupId { get; set; }
    }
}
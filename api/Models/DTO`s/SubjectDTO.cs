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
        public string Description { get; set; } = null!;

        public Guid TeacherId { get; set; }
        public int? GroupId { get; set; }
    }
}
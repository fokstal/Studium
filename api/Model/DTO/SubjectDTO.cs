using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class SubjectDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(65)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Descripton { get; set; } = null!;

        [Required]
        [MaxLength(60)]
        public string TeacherName { get; set; } = null!;
        public int? GroupId { get; set; }
    }
}
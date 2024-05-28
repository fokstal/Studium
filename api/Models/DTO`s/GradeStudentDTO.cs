using System.ComponentModel.DataAnnotations;
using api.Helpers.Enums;

namespace api.Models.DTO
{
    public class GradeStudentDTO
    {
        [Required]
        public Guid StudentId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public GradeTypeEnum Type { get; set; }
        
        [Required]
        public DateTime SetDate { get; set; }

        [Range(-1, 10)]
        public int Value { get; set; }
    }
}
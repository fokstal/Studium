using System.ComponentModel.DataAnnotations;
using api.Helpers.Enums;

namespace api.Models.DTO
{
    public class GradeStudentDTO
    {
        [Required]
        public Guid StudentEntityId { get; set; }

        [Required]
        public int SubjectEntityId { get; set; }

        [Required]
        public GradeTypeEnum TypeEnum { get; set; }
        
        [Required]
        public DateTime SetDate { get; set; }

        [Range(-1, 10)]
        public int Value { get; set; }
    }
}
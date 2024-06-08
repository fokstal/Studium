using System.ComponentModel.DataAnnotations;
using api.Helpers.Enums;
using api.Models.Entities;

namespace api.Models.DTO
{
    public class GradesDTO
    {
        [Required]
        public int SubjectId { get; set; }

        [Required]
        public GradeTypeEnum Type { get; set; }

        [Required]
        public DateTime SetDate { get; set; }

        [Required]
        public List<StudentToValueEntity> StudentToValueList { get; set; } = [];
    }
}
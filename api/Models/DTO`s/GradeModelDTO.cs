using System.ComponentModel.DataAnnotations;
using api.Helpers.Enums;
using api.Models.Entities;

namespace api.Models.DTO
{
    public class GradeModelDTO
    {
        [Required]
        public int SubjectId { get; set; }

        [Required]
        public GradeTypeEnum Type { get; set; }

        [Required]
        public DateTime SetDate { get; set; }

        [Required]
        public HashSet<GradeEntity> StudentToValueList { get; set; } = [];
    }
}
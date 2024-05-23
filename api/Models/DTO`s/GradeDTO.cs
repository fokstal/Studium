using System.ComponentModel.DataAnnotations;
using api.Helpers.Enums;

namespace api.Model.DTO
{
    public class GradeDTO
    {
        [Required]
        [Range(-1, 10)]
        public int Value { get; set; }

        [Required]
        public Guid StudentId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public GradeTypeEnum Type { get; set; }
    }
}
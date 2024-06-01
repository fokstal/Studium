using System.ComponentModel.DataAnnotations;
using api.Helpers.Enums;

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
        public List<GradeDTO> GradeList { get; set; } = [];
    }
}
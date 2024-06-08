using System.ComponentModel.DataAnnotations;
using api.Helpers.Enums;

namespace api.Models.DTO
{
    public class GradeModelDTO
    {
        [Required]
        public int SubjectEntityId { get; set; }

        [Required]
        public GradeTypeEnum TypeEnum { get; set; }

        [Required]
        public DateTime SetDate { get; set; }

        [Required]
        public List<GradeDTO> GradeDTOList { get; set; } = [];
    }
}
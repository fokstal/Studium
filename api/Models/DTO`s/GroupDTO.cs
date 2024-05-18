using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class GroupDTO
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(70)]
        public string Description { get; set; } = null!;

        public int? CuratorId { get; set; }

        [Required]
        [MaxLength(5)]
        public string AuditoryName { get; set; } = null!;
    }
}
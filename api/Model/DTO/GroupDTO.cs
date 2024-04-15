using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class GroupDTO
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string Name { get; set; } = "~группа~";

        [MaxLength(70)]
        public string Description { get; set; } = "~описание~";

        [MaxLength(80)]
        public string Curator { get; set; } = "~куратор~";

        [MaxLength(5)]
        public string AuditoryName { get; set; } = "~№0~";
    }
}
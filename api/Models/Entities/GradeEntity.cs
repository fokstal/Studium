using api.Models.Entities;

namespace api.Models
{
    public class GradeEntity : ModelEntity
    {
        public int Value { get; set; }
        public DateTime SetDate { get; set; }
        public Guid StudentId { get; set; }
        public int SubjectId { get; set; }
        public GradeTypeEntity Type { get; set; } = null!;
    }
}
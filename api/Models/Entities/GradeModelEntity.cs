using api.Models.Entities;

namespace api.Models
{
    public class GradeModelEntity : ModelEntity
    {
        public int SubjectId { get; set; }
        public DateTime SetDate { get; set; }

        public int TypeId { get; set;}
        public GradeTypeEntity Type { get; set; } = null!;
        public HashSet<GradeEntity> GradeList { get; set; } = [];
    }
}
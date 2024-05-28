using api.Models.Entities;

namespace api.Models
{
    public class GradesEntity : ModelEntity
    {
        public int SubjectId { get; set; }
        public GradeTypeEntity Type { get; set; } = null!;
        public DateTime SetDate { get; set; }
        public List<StudentToValueEntity> StudentToValueList { get; set; } = [];
    }
}
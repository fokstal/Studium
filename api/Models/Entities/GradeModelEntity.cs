using api.Models.Entities;

namespace api.Models
{
    public class GradeModelEntity : ModelEntity
    {
        public new Guid Id { get; set; }
        public DateTime SetDate { get; set; }

        public int SubjectEntityId { get; set; }
        public int TypeId { get; set;}
        public GradeTypeEntity Type { get; set; } = null!;
        public List<GradeEntity> GradeEntityList { get; set; } = [];
    }
}
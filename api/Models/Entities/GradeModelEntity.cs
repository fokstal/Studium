using api.Models.Entities;

namespace api.Models
{
    public class GradeModelEntity : ModelEntity
    {
        public new Guid Id { get; set; }
        public DateTime SetDate { get; set; }

        public int SubjectEntityId { get; set; }
        public GradeTypeEntity TypeEntity { get; set; } = null!;
        public List<GradeEntity> GradeEntityList { get; set; } = [];
    }
}
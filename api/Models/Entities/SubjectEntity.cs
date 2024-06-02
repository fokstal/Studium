namespace api.Models
{
    public class SubjectEntity : ModelEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid TeacherId { get; set; }

        public int GroupEntityId { get; set; }
        public List<GradeModelEntity> GradeModelEntityList { get; set; } = [];
    }
}
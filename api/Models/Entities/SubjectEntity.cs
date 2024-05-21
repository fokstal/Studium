namespace api.Models
{
    public class SubjectEntity : ModelEntity
    {
        public string Name { get; set; } = null!;
        public string Descripton { get; set; } = null!;
        public Guid TeacherId { get; set; }

        public int? GroupId { get; set; }
        public List<GradeEntity> GradeList { get; set; } = [];
    }
}
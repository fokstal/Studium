namespace api.Models
{
    public class SubjectEntity : ModelEntity
    {
        public string Name { get; set; } = null!;
        public string Descripton { get; set; } = null!;
        public string TeacherName { get; set; } = null!;

        public int? GroupId { get; set; }
        public List<GradeEntity> GradeList { get; set; } = [];
    }
}
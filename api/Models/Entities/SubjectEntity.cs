namespace api.Models
{
    public class SubjectEntity : IModelEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Descripton { get; set; } = null!;
        public string TeacherName { get; set; } = null!;

        public int? GroupId { get; set; }
        public List<GradeEntity> GradeList { get; set; } = [];
    }
}
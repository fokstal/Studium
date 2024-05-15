namespace api.Models
{
    public class GroupEntity : IModelEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Curator { get; set; } = null!;
        public string AuditoryName { get; set; } = null!;

        public List<StudentEntity> StudentList { get; set; } = [];
        public List<SubjectEntity> SubjectList { get; set; } = [];
    }
}
using api.Models;

namespace api.Model
{
    public class Group : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Curator { get; set; } = null!;
        public string AuditoryName { get; set; } = null!;

        public List<Student> StudentList { get; set; } = [];
        public List<Subject> SubjectList { get; set; } = [];
    }
}
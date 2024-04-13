namespace api.Model
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Descripton { get; set; } = null!;
        public string TeacherName { get; set; } = null!;

        public Grade[] GradeList { get; set; } = [];
    }
}
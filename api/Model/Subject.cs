using System.ComponentModel.DataAnnotations;

namespace api.Model
{
    public class Subject
    {
        public int Id { get; set; }

        [MaxLength(65)]
        public string Name { get; set; } = "~предмет~";

        [MaxLength(150)]
        public string Descripton { get; set; } = "~описание~";

        [MaxLength(60)]
        public string TeacherName { get; set; } = "~преподаватель~";

        public Grade[] GradeList { get; set; } = [];
    }
}
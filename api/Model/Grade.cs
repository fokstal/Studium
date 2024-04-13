using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model
{
    public class Grade
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime SetDate { get; set; }
        
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; } = null!;
    }
}
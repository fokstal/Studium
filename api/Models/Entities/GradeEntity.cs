namespace api.Models
{
    public class GradeEntity : ModelEntity
    {
        public int Value { get; set; }
        public DateTime SetDate { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
    }
}
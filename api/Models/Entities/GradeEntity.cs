namespace api.Models
{
    public class GradeEntity : IModelEntity
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime SetDate { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
    }
}
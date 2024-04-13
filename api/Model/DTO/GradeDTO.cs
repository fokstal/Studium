namespace api.Model.DTO
{
    public class GradeDTO
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime SetDate { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
    }
}
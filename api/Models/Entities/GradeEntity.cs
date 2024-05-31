namespace api.Models.Entities
{
    public class GradeEntity
    {
        public int Id { get; set; }
        public Guid StudentId { get; set; }
        public int Value { get; set; }
        
        public Guid GradeModelId { get; set; }
    }
}
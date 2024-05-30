namespace api.Models.Entities
{
    public class StudentToValueEntity
    {
        public int Id { get; set; }
        public Guid StudentId { get; set; }
        public int Value { get; set; }
        
        public int GradesEntityId { get; set; }
    }
}
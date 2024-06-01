namespace api.Models.Entities
{
    public class GradeEntity
    {
        public int Id { get; set; }
        public int Value { get; set; }
        
        public Guid StudentEntityId { get; set; }
        public Guid GradeModelEntityId { get; set; }
    }
}
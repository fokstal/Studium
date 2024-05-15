namespace api.Models
{
    public class StudentEntity : IModelEntity
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public int? GroupId { get; set; }
    }
}
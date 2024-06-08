namespace api.Models
{
    public class StudentEntity : ModelEntity
    {
        public new Guid Id { get; set; }
        public DateTime AddedDate { get; set;}
        public DateTime RemovedDate { get; set; }

        public int PersonEntityId { get; set; }
        public int? GroupEntityId { get; set; }
    }
}
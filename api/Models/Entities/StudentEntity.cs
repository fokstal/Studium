namespace api.Models
{
    public class StudentEntity : ModelEntity
    {
        public DateTime AddedDate { get; set;}
        public DateTime RemovedDate { get; set; }

        public int PersonId { get; set; }
        public int? GroupId { get; set; }
    }
}
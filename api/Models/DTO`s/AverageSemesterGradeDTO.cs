namespace api.Models.DTO
{
    public class AverageSemesterGradeDTO
    {
        public Guid StudentEntityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
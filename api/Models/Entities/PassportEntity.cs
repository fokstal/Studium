namespace api.Models
{
    public class PassportEntity : ModelEntity
    {
        public string ScanFileName { get; set; } = null!;
        public int PersonEntityId { get; set; }
    }
}
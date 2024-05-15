namespace api.Models
{
    public class PassportEntity : IModelEntity
    {
        public int Id { get; set; }
        public string ScanFileName { get; set; } = null!;
        public int PersonId { get; set; }
    }
}
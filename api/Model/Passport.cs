namespace api.Model
{
    public class Passport
    {
        public int Id { get; set; }
        public string ScanFileName { get; set; } = null!;
        public int PersonId { get; set; }
    }
}
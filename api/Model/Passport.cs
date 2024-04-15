namespace api.Model
{
    public class Passport
    {
        public int Id { get; set; }
        public string Photo { get; set; } = null!;
        public int PersonId { get; set; }
    }
}
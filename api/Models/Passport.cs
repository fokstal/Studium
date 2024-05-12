using api.Models;

namespace api.Model
{
    public class Passport : IModel
    {
        public int Id { get; set; }
        public string ScanFileName { get; set; } = null!;
        public int PersonId { get; set; }
    }
}
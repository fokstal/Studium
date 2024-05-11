using api.Models;

namespace api.Model
{
    public class Grade : IModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime SetDate { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
    }
}
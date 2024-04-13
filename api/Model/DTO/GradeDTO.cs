using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class GradeDTO
    {
        public int Id { get; set; }

        [Range(-1, 10)]
        public int Value { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
    }
}
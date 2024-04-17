using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class PassportDTO
    {
        public int Id { get; set; }
        [Required]
        public IFormFile Scan { get; set; } = null!;
        public int PersonId { get; set; }
    }
}
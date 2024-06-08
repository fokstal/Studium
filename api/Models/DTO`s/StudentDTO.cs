using System.ComponentModel.DataAnnotations;

namespace api.Model.DTO
{
    public class StudentDTO
    {
        public Guid Id { get; set;}

        [Required]
        public DateTime AddedDate { get; set;}
        
        [Required]
        public DateTime RemovedDate { get; set; }

        [Required]
        public int PersonEntityId { get; set; }
        public int? GroupEntityId { get; set; }
    }
}
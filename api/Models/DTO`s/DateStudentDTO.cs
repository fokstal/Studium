using System.ComponentModel.DataAnnotations;

namespace api.Models.DTO
{
    public class DateStudentDTO
    {
        [Required]
        public DateTime AddedDate { get; set;}
        
        [Required]
        public DateTime RemovedDate { get; set; }
    }
}
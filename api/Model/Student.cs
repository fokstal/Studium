using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model
{
    public class Student
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        [ForeignKey(nameof(PersonId))]
        public Person Person { get; set; } = null!;

        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; } = null!;
    }
}
using api.Models;

namespace api.Model
{
    public class Student : IModel
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public int? GroupId { get; set; }
    }
}
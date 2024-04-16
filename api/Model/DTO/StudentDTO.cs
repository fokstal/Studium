namespace api.Model.DTO
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string PersonLastName { get; set; } = "~фамилия~";
        public string GroupName { get; set; } = "~группа~";
    }
}
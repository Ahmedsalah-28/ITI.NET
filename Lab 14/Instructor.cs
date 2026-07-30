using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1
{
    public class Instructor
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = "";
        public int YearsOfExperience { get; set; }
        //public string? AssignedCourseName { get; set; }
        public List<Course> Courses { get; set; } = new();

    }


}
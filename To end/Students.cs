using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    
    public class Student // class = table
    {
        public int Id { get; set; } // property = column  // StudentId or Id
        public string FullName { get; set; } = "";
        public int YearOfStudy { get; set; }
        public double Gpa { get; set; }

        public int CreditsCompleted { get; set; } // Part G Lab ID 7 mod 3 = 1
    }


}
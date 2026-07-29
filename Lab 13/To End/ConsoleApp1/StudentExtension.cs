using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ConsoleApp1
{
    public static class StudentExtensions
    {
        // Threshold = 3.4

        public static IEnumerable<Student> MyTopStudents(this IEnumerable<Student> students)
        {
            return students.Where(s => s.Gpa >= 3.4);
        }
    }
}



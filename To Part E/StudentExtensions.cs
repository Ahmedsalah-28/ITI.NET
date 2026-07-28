using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace ConsoleApp2
{
    public static class StudentExtensions
    {
        // Threshold = 3.4

        public static IEnumerable<Student> MyTopStudents(this IEnumerable<Student> students)
        {
            return students.Where(s => s.GPA >= 3.4);
        }
    }
}



using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;

namespace ConsoleApp2
{

    interface IPrintable
    {
        void PrintDetails();
    }

    interface IRankable
    {
        int GetRankScore();
    }
    class Course : IPrintable
    {
        private string courseName;
        private int credits;
        private static int totalCourses;
        public string CourseName
        {
            get { return courseName; }
            set { courseName = value; }
        }
        public int Credits
        {
            get { return credits; }
            set
            { if (value >= 1 && value <= 6) { credits = value; } }
        }
        public Course(string courseName, int credits)
        { CourseName = courseName; Credits = credits; totalCourses++; }
        public void PrintSummary()
        { Console.WriteLine($"Course : {CourseName} Credits : {Credits}"); }
        public static int GetTotalCourses()
        {
            return totalCourses;
        }

        public void PrintDetails()
        {
            PrintSummary();
        }
    }

    public abstract class Person
    {
        protected string fullName;

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public Person(string fullName)
        {
            FullName = fullName;
        }


        // Lab ID = 7
        // 7 % 3 = 1
        // 1 + 2 = 3
        // This method is protected because only derived classes should use it.
        // If it were private, Student and Instructor could not access it.
        // If it were public, anyone could call it, even though it is only an internal helper.
        protected string FormatTag()
        {
            //int lab_id = 7;
            //int num_lab = (7%3)+2;
            if (fullName.Length < 3)
                return fullName;

            return fullName.Substring(0, 3).ToUpper();
        }

        public virtual void PrintBasicInfo()
        {
            Console.WriteLine($"Name : {FullName}");
            Console.WriteLine($"Tag  : {FormatTag()}");
        }

        public abstract string GetRoleDescription();
    }


    class Instructor : Person, IPrintable
    {
        private int yearsOfExperience;
        private string assignedCourseName;

        public int YearsOfExperience
        {
            get { return yearsOfExperience; }
            set
            {
                if (value >= 0)
                    yearsOfExperience = value;
            }
        }

        public string AssignedCourseName
        {
            get { return assignedCourseName; }
            set { assignedCourseName = value; }
        }

        public Instructor(string fullName, int yearsOfExperience)
            : base(fullName)
        {
            YearsOfExperience = yearsOfExperience;
            AssignedCourseName = "";
        }

        public override void PrintBasicInfo()
        {
            base.PrintBasicInfo();

            Console.WriteLine($"Experience : {YearsOfExperience} Years");

            if (string.IsNullOrWhiteSpace(AssignedCourseName))
                Console.WriteLine("Assigned Course : None");
            else
                Console.WriteLine($"Assigned Course : {AssignedCourseName}");
        }

        public override string GetRoleDescription()
        {
            return "Instructor";
        }

        public void PrintSummary()
        {

            Console.WriteLine("-------------------------");

            PrintBasicInfo();

            Console.WriteLine($"Role : {GetRoleDescription()}");

        }

        public void PrintDetails()
        {
            PrintSummary();
        }
    }

    public class Student : Person, IPrintable
    {
        private int year;
        private double gpa;

        private static int totalStudents;

        public int Year
        {
            get { return year; }
            set
            {
                if (value >= 1 && value <= 4)
                    year = value;
            }
        }

        public double GPA
        {
            get { return gpa; }
            set
            {
                if (value >= 0 && value <= 4)
                    gpa = value;
            }
        }

        public Student(string fullName, int year, double gpa) : base(fullName)
        {
            Year = year;
            GPA = gpa;

            totalStudents++;
        }

        public static int GetTotalStudents()
        {
            return totalStudents;
        }

        public string ClassifyYear()
        {
            switch (Year)
            {
                case 1: return "Freshman";
                case 2: return "Sophomore";
                case 3: return "Junior";
                case 4: return "Senior";
                default: return "Unknown";
            }
        }

        public string ClassifyHonorStatus()
        {
            if (GPA >= 3.5)
                return "Dean's List";
            else if (GPA >= 3.0)
                return "Honor Roll";
            else
                return "Standard Standing";
        }
        public override void PrintBasicInfo()
        {
            base.PrintBasicInfo();

            Console.WriteLine($"Year   : {ClassifyYear()}");
            Console.WriteLine($"GPA    : {GPA:F2}");
            Console.WriteLine($"Status : {ClassifyHonorStatus()}");
        }

        public override string GetRoleDescription()
        {
            return "Student";
        }

        public void PrintSummary()
        {

            Console.WriteLine("-------------------------");

            PrintBasicInfo();

            Console.WriteLine($"Role : {GetRoleDescription()}");

        }

        public void PrintDetails()
        {
            PrintSummary();
        }
    }


    class Admin : Person, IPrintable, IRankable
    {
        private int accessLevel;

        // Lab ID = 7
        // Access Level Range = 1 to 3
        public int AccessLevel
        {
            get { return accessLevel; }
            set
            {
                if (value >= 1 && value <= 3)
                    accessLevel = value;
            }
        }

        public Admin(string fullName, int accessLevel)
            : base(fullName)
        {
            AccessLevel = accessLevel;
        }

        public override void PrintBasicInfo()
        {
            base.PrintBasicInfo();

            Console.WriteLine($"Access Level : {AccessLevel}");
        }

        public override string GetRoleDescription()
        {
            return "Admin";
        }

        public void PrintSummary()
        {
            Console.WriteLine("-------------------------");

            PrintBasicInfo();

            Console.WriteLine($"Role  : {GetRoleDescription()}");
            Console.WriteLine($"Score : {GetRankScore()}");
        }

        public void PrintDetails()
        {
            PrintSummary();

        }
        public int GetRankScore()
        {
            return 4; // (7 % 4) + 1 = 4
        }

    }


    internal class Program
    {

    
        // Threshold = 2.0 + ((7 % 5) * 0.4)
        // = 2.0 + (2 * 0.4)
        // = 2.8
        static List<Student> FilterStudents(List<Student> students, Func<Student, bool> condition)
        {
            List<Student> result = new List<Student>();

            foreach (Student student in students)
            {
                if (condition(student))
                {
                    result.Add(student);
                }
            }

            return result;
        }
        static bool IsAboveMyThreshold(Student s) { return s.GPA > 2.8; }   // Threshold = 2.8 
        static void ApplyToAll(List<Student> students, Action<Student> action)
        {
            foreach (Student student in students)
            {
                action(student);
            }
        }


        // where T : class
        // This constraint is required because only reference types can return null.
        static T? FindFirst<T>(List<T> items, Func<T, bool> condition)
            where T : class
        {
            foreach (T item in items)
            {
                if (condition(item))
                    return item;
            }

            return null;
        }
        //static bool IsTopStudent(Student s){ return s.GPA > 3.5;}

        class Tracker<T>
        {
            private List<T> items = new List<T>();

            public void Add(T item)
            {
                // Capacity = 3
                if (items.Count >= 3)
                {
                    Console.WriteLine("Tracker is Full.");
                    return;
                }

                items.Add(item);

                Console.WriteLine($"Count = {items.Count}");
            }

            public List<T> GetAll()
            {
                return items;
            }
        }
        static void Main(string[] args)
        {
            //List<Student> students = new List<Student>
            //    {
            //        new Student("Ahmed",2,3.5),
            //        new Student("salah",3,2.8),
            //        new Student("farouk",1,3.9),
            //        new Student("mo",4,3.2)
            //    };

            //List<Instructor> instructors = new List<Instructor>
            //    {
            //        new Instructor("Hamdy", 10),
            //        new Instructor("Mona Khalil", 6)
            //    };
            //List<Course> courses = new List<Course>
            //    {
            //        new Course("Web Development Using .NET", 4),
            //        new Course("Database Fundamentals", 3)
            //    };



            List<Student> students = new List<Student>
                {
                    new Student("Ahmed",2,3.5),
                    new Student("salah",3,2.8),
                    new Student("farouk",1,3.9),
                    new Student("mo",4,3.2)
                };

            //C1) Print all seven aggregates
            // Threshold = 2.5 + ((7 % 4) * 0.3)
            // = 2.5 + (3 * 0.3)
            // = 3.4
            //C1)
            // Threshold = 3.4

            Console.WriteLine($"Total Students = {students.Count()}");

            Console.WriteLine($"Students Above Threshold = {students.Count(s => s.GPA > 3.4)}");

            Console.WriteLine($"Average GPA = {students.Average(s => s.GPA):F2}");

            Console.WriteLine($"Highest GPA = {students.Max(s => s.GPA):F2}");

            Console.WriteLine($"Lowest GPA = {students.Min(s => s.GPA):F2}");

            Console.WriteLine($"Any Student Below 2.0 = {students.Any(s => s.GPA < 2.0)}");

            Console.WriteLine($"All Students >= 2.0 = {students.All(s => s.GPA >= 2.0)}");

            //C2) Trigger the exception

            List<Student> emptyStudents = new List<Student>();

            Console.WriteLine(emptyStudents.Count());

            Console.WriteLine(emptyStudents.Any());

            // This throws an exception
            //Console.WriteLine(emptyStudents.Average(s => s.GPA));

            // Exception Type: InvalidOperationException
            // Message: Sequence contains no elements.

            //C3) Fix with a guard
            if (emptyStudents.Any())
            {
                Console.WriteLine(emptyStudents.Average(s => s.GPA));
            }
            else
            {
                Console.WriteLine("The collection is empty. Average cannot be calculated.");
            }

            //C4) Group by Year
            var groups = students.GroupBy(s => s.Year);

            foreach (var group in groups)
            {
                Console.WriteLine($"\nYear {group.Key}");
                Console.WriteLine($"Count = {group.Count()}");

                foreach (var student in group)
                {
                    Console.WriteLine($"{student.FullName} - {student.GPA:F2}");
                }
            }

            // The groups are not automatically sorted.
            // GroupBy preserves the order in which each key first appears
            // in the original collection.


            //C5) Group using your own key
            // Threshold = 3.4

            var customGroups = students.GroupBy(s =>
                s.GPA >= 3.4 ? "High Performers" : "Needs Improvement");

            foreach (var group in customGroups)
            {
                Console.WriteLine($"\n{group.Key}");
                Console.WriteLine($"Count = {group.Count()}");

                foreach (var student in group)
                {
                    Console.WriteLine($"{student.FullName} - {student.GPA:F2}");
                }
            }

            //C6) Sort groups by key

            var sortedGroups = students.GroupBy(s => s.Year)
                                        .OrderBy(g => g.Key);

            foreach (var group in sortedGroups)
            {
                Console.WriteLine($"\nYear {group.Key}");
                Console.WriteLine($"Count = {group.Count()}");

                foreach (var student in group)
                {
                    Console.WriteLine($"{student.FullName} - {student.GPA:F2}");
                }
            }

            // I added OrderBy(g => g.Key) after GroupBy()
            // to sort the groups by their year (group key).





            Console.WriteLine("--------------Part D--------------------");
            Console.WriteLine();

            List<Student> students2 = new List<Student>
                {
                    new Student("Yara Adel", 2, 3.5),
                    new Student("Omar Hesham", 3, 2.8),
                    new Student("Nada Samir", 1, 3.9),
                    new Student("Kareem Fouad", 4, 3.2)
                };
            List<Instructor> instructors = new List<Instructor>
                {
                    new Instructor("Hamdy", 10)
                    {
                        AssignedCourseName = "Web Development Using .NET"
                    },

                    new Instructor("Mona Khalil", 6)
                    {
                        AssignedCourseName = "Database Fundamentals"
                    }
                };
            List<Course> courses = new List<Course>
                {
                    new Course("Web Development Using .NET", 4),
                    new Course("Database Fundamentals", 3)
                };

            //Q1) Method Syntax Join
            var result = instructors.Join(
                    courses,
                    instructor => instructor.AssignedCourseName,
                    course => course.CourseName,
                    (instructor, course) => new
                    {
                        InstructorName = instructor.FullName,
                        CourseName = course.CourseName,
                        Credits = course.Credits
                    });

            Console.WriteLine("===== Method Syntax =====");

            foreach (var item in result)
            {
                Console.WriteLine($"{item.InstructorName} - {item.CourseName} ({item.Credits} Credits)");
            }

            //Q2) Query Syntax Join
            var result2 =
                    from instructor in instructors
                    join course in courses
                    on instructor.AssignedCourseName equals course.CourseName
                    select new
                    {
                        InstructorName = instructor.FullName,
                        CourseName = course.CourseName,
                        Credits = course.Credits
                    };

            Console.WriteLine("\n===== Query Syntax =====");

            foreach (var item in result2)
            {
                Console.WriteLine($"{item.InstructorName} - {item.CourseName} ({item.Credits} Credits)");
            }

            //Q3)
            //(7 % 5) + 3 = 5

            instructors.Add(
                new Instructor("Ahmed Salah Farouk", 5)
                {
                    AssignedCourseName = "Machine Learning"
                });
            //Q4) Re-run the Join
            var result3 = instructors.Join(
                        courses,
                        instructor => instructor.AssignedCourseName,
                        course => course.CourseName,
                        (instructor, course) => new
                        {
                            InstructorName = instructor.FullName,
                            CourseName = course.CourseName,
                            Credits = course.Credits
                        });

            Console.WriteLine("\n===== After Adding Ahmed =====");

            Console.WriteLine($"Instructors Count = {instructors.Count}");
            Console.WriteLine($"Join Rows = {result3.Count()}");

            foreach (var item in result3)
            {
                Console.WriteLine($"{item.InstructorName} - {item.CourseName} ({item.Credits} Credits)");
            }

            // Join returns only matching records.
            // Ahmed is assigned to "Machine Learning",
            // but no course with that name exists.
            // Therefore, he is not included in the result.
            // No exception is thrown because Join simply skips unmatched items.


            //Q5)

            // To include instructors even when they have no matching course,
            // we would use a Left Outer Join.
            // In LINQ, this is implemented using GroupJoin()
            // together with DefaultIfEmpty().



            Console.WriteLine("--------------Part E--------------------");
            Console.WriteLine();

            //Q1) Prove Deferred Execution
            // Prediction BEFORE running:
            // I predict the count will be 4 because the query is deferred.
            // Layla will be included when Count() executes.

            var highGpaStudents = students2.Where(s => s.GPA > 3.0);

            students2.Add(new Student("Layla Mostafa", 2, 3.7));

            Console.WriteLine($"Count = {highGpaStudents.Count()}");

            // Result:
            // My prediction was correct. The count is 4 because the query
            // was executed after Layla was added.

            //Q2) Remove Layla

            students2.RemoveAll(s => s.FullName == "Layla Mostafa");


            //Q3) 
            var query = students2.Where(s=>s.GPA > 3.0);

            Console.WriteLine($"Count = {query.Count()}");

            foreach (var student in query)
            {
                Console.WriteLine(student.FullName);
            }

            Console.WriteLine($"Average = {query.Average(s => s.GPA):F2}");

            // The filtering runs three times:
            // 1. Count()
            // 2. foreach
            // 3. Average()
            // because the query is executed every time it is enumerated.

            //Q4) Fix It
            Console.WriteLine("-------------fixedQuery------------------");
            var fixedQuery = students2
                .Where(s => s.GPA > 3.0)
                .ToList();

            Console.WriteLine($"Count = {fixedQuery.Count}");

            foreach (var student in fixedQuery)
            {
                Console.WriteLine(student.FullName);
            }

            Console.WriteLine($"Average = {fixedQuery.Average(s => s.GPA):F2}");
            // ToList() executes the query once and stores the results.
            // Count(), foreach, and Average() now work on the same list
            // without running the filter again.
            // This matters much more in Part H because repeated enumeration
            // of a database query would execute multiple SQL queries.

            //Q5) Extension Method

            var topStudents = students2
                .MyTopStudents()
                .OrderBy(s => s.FullName)
                .Select(s => s.FullName)
                .ToList();

            Console.WriteLine("Top Students:");

            foreach (var name in topStudents)
            {
                Console.WriteLine(name);
            }

            // MyTopStudents() is deferred because it returns IEnumerable<Student>.
            // It only builds the query. The filtering is executed later when the
            // sequence is enumerated (for example by ToList(), Count(), or foreach).


        }

        //static void PrintAllNames<T>(List<T> items)
        //{
        //    foreach (T item in items)
        //    {
        //        Console.WriteLine(item.FullName);



        //    }
        //}
        // The compiler does not know that T has a FullName property
        static void PrintAllNames<T>(List<T> items) where T : Person
        {
            foreach (T item in items)
            {
                Console.WriteLine(item.FullName);
            }
        }






    }

  
}

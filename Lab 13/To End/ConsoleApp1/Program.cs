//Part F

//F3
// Q1:
// The Up() method creates three tables:
// 1. Students
// 2. Instructors
// 3. Courses

// Q2:
// EF mapped Gpa to SQL type float.
// EF mapped FullName to SQL type nvarchar(max).

// Q3:
// No, FullName is not nullable.
// I did not explicitly configure it.
// EF inferred that because FullName is a non-nullable string property.

// Q4:
// The Down() method would remove the Courses, Instructors, and Students tables from the database.


// Q5:
// The database does not exist yet.
// This proves that Add-Migration only generates migration files.
// It does not create the database.



// The database and its tables were successfully created
// after running Update-Database.

//1
// The EF table includes an Identity primary key (Id),
// which is generated automatically.

//2
// In Session 3, the column sizes were specified manually.
// In EF Core, FullName was generated as nvarchar(max) automatically.





//Part G
/*
Up() modifies the existing Students table.
It adds the CreditsCompleted column.
It is not a CreateTable operation because the Students table already exists.
This matters because the existing student rows are preserved.
Only the table structure changes, while the data remains.
*/

/*
The Students table still contains 4 rows after the migration.
The CreditsCompleted column was added successfully.
*/



namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {


            List<Student> students = new List<Student>
                    {
                        new Student
                        {
                            FullName = "Yara Adel",
                            YearOfStudy = 2,
                            Gpa = 3.5,
                            CreditsCompleted = 60
                        },

                        new Student
                        {
                            FullName = "Omar Hesham",
                            YearOfStudy = 3,
                            Gpa = 2.8,
                            CreditsCompleted = 90
                        },

                        new Student
                        {
                            FullName = "Nada Samir",
                            YearOfStudy = 1,
                            Gpa = 3.9,
                            CreditsCompleted = 30
                        },

                        new Student
                        {
                            FullName = "Kareem Fouad",
                            YearOfStudy = 4,
                            Gpa = 3.2,
                            CreditsCompleted = 120
                        }
                    };
            //part c
            // Threshold = 2.5 + ((7 % 4) * 0.3)
            // = 2.5 + (3 * 0.3)
            // = 3.4

            Console.WriteLine($"Total Students = {students.Count()}");

            Console.WriteLine($"Students Above Threshold = {students.Count(s => s.Gpa > 3.4)}");

            Console.WriteLine($"Average GPA = {students.Average(s => s.Gpa):F2}");

            Console.WriteLine($"Highest GPA = {students.Max(s => s.Gpa):F2}");

            Console.WriteLine($"Lowest GPA = {students.Min(s => s.Gpa):F2}");

            Console.WriteLine($"Any Student Below 2.0 = {students.Any(s => s.Gpa < 2.0)}");

            Console.WriteLine($"All Students >= 2.0 = {students.All(s => s.Gpa >= 2.0)}");

            //C2) Trigger the exception

            List<Student> emptyStudents = new List<Student>();

            Console.WriteLine(emptyStudents.Count());

            Console.WriteLine(emptyStudents.Any());

            // This throws an exception
            //Console.WriteLine(emptyStudents.Average(s => s.Gpa));

            // Exception Type: InvalidOperationException
            // Message: Sequence contains no elements.

            //C3) Fix with a guard
            if (emptyStudents.Any())
            {
                Console.WriteLine(emptyStudents.Average(s => s.Gpa));
            }
            else
            {
                Console.WriteLine("The collection is empty. Average cannot be calculated.");
            }

            //C4) Group by Year
            var groups = students.GroupBy(s => s.YearOfStudy);

            foreach (var group in groups)
            {
                Console.WriteLine($"\nYear {group.Key}");
                Console.WriteLine($"Count = {group.Count()}");

                foreach (var student in group)
                {
                    Console.WriteLine($"{student.FullName} - {student.Gpa:F2}");
                }
            }

            // The groups are not automatically sorted.
            // GroupBy preserves the order in which each key first appears
            // in the original collection.


            //C5) Group using key
            // Threshold = 3.4

            var customGroups = students.GroupBy(s =>
                s.Gpa >= 3.4 ? "High Performers" : "Needs Improvement");

            foreach (var group in customGroups)
            {
                Console.WriteLine($"\n{group.Key}");
                Console.WriteLine($"Count = {group.Count()}");

                foreach (var student in group)
                {
                    Console.WriteLine($"{student.FullName} - {student.Gpa:F2}");
                }
            }

            //C6) Sort groups by key

            var sortedGroups = students.GroupBy(s => s.YearOfStudy)
                                        .OrderBy(g => g.Key);

            foreach (var group in sortedGroups)
            {
                Console.WriteLine($"\nYear {group.Key}");
                Console.WriteLine($"Count = {group.Count()}");

                foreach (var student in group)
                {
                    Console.WriteLine($"{student.FullName} - {student.Gpa:F2}");
                }
            }

            // I added OrderBy(g => g.Key) after GroupBy()
            // to sort the groups by their year (group key).


            Console.WriteLine("--------------Part D--------------------");
            Console.WriteLine();


            List<Instructor> instructors = new List<Instructor>
{
    new Instructor
    {
        FullName = "Hamdy",
        YearsOfExperience = 10,
        AssignedCourseName = "Web Development Using .NET"
    },

    new Instructor
    {
        FullName = "Mona Khalil",
        YearsOfExperience = 6,
        AssignedCourseName = "Database Fundamentals"
    }
};

            List<Course> courses = new List<Course>
{
    new Course
    {
        CourseName = "Web Development Using .NET",
        Credits = 4
    },

    new Course
    {
        CourseName = "Database Fundamentals",
        Credits = 3
    }
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

            new Instructor
            {
                FullName = "Ahmed Salah Farouk",
                YearsOfExperience = 5,
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

            var highGpaStudents = students.Where(s => s.Gpa > 3.0);

            students.Add(new Student
            {
                FullName = "Yara Adel",
                YearOfStudy = 2,
                Gpa = 3.5,
                CreditsCompleted = 60
            });



            Console.WriteLine($"Count = {highGpaStudents.Count()}");

            // Result:
            // My prediction was correct. The count is 4 because the query
            // was executed after Layla was added.

            //Q2) Remove Layla

            students.RemoveAll(s => s.FullName == "Layla Mostafa");


            //Q3) 
            var query = students.Where(s => s.Gpa > 3.0);

            Console.WriteLine($"Count = {query.Count()}");

            foreach (var student in query)
            {
                Console.WriteLine(student.FullName);
            }

            Console.WriteLine($"Average = {query.Average(s => s.Gpa):F2}");

            // The filtering runs three times:
            // 1. Count()
            // 2. foreach
            // 3. Average()
            // because the query is executed every time it is enumerated.

            //Q4) Fix It
            Console.WriteLine("-------------fixedQuery------------------");
            var fixedQuery = students
                .Where(s => s.Gpa > 3.0)
                .ToList();

            Console.WriteLine($"Count = {fixedQuery.Count}");

            foreach (var student in fixedQuery)
            {
                Console.WriteLine(student.FullName);
            }

            Console.WriteLine($"Average = {fixedQuery.Average(s => s.Gpa):F2}");
            // ToList() executes the query once and stores the results.
            // Count(), foreach, and Average() now work on the same list
            // without running the filter again.
            // This matters much more in Part H because repeated enumeration
            // of a database query would execute multiple SQL queries.

            //Q5) Extension Method

            var topStudents = students
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









            //using (StudentPortalContext db = new StudentPortalContext())
            //{

            //    db.Students.AddRange(
            //          new Student
            //          {
            //              FullName = "Yara Adel",
            //              YearOfStudy = 2,
            //              Gpa = 3.5
            //          },
            //          new Student
            //          {
            //              FullName = "Omar Hesham",
            //              YearOfStudy = 3,
            //              Gpa = 2.8
            //          },
            //          new Student
            //          {
            //              FullName = "Nada Samir",
            //              YearOfStudy = 1,
            //              Gpa = 3.9
            //          },
            //          new Student
            //          {
            //              FullName = "Kareem Fouad",
            //              YearOfStudy = 4,
            //              Gpa = 3.2
            //          });

            //    db.SaveChanges();
            //}


            //--------------------Part H ---------------------

            //using (StudentPortalContext context = new StudentPortalContext())
            //{
            //    if (!context.Students.Any())
            //    {
            //        context.Students.AddRange(
            //            new Student
            //            {
            //                FullName = "Yara Adel",
            //                YearOfStudy = 2,
            //                Gpa = 3.5,
            //                CreditsCompleted = 60
            //            },
            //            new Student
            //            {
            //                FullName = "Omar Hesham",
            //                YearOfStudy = 3,
            //                Gpa = 2.8,
            //                CreditsCompleted = 90
            //            },
            //            new Student
            //            {
            //                FullName = "Nada Samir",
            //                YearOfStudy = 1,
            //                Gpa = 3.9,
            //                CreditsCompleted = 30
            //            },
            //            new Student
            //            {
            //                FullName = "Kareem Fouad",
            //                YearOfStudy = 4,
            //                Gpa = 3.2,
            //                CreditsCompleted = 120
            //            });

            //        context.SaveChanges();

            //        Console.WriteLine("Students Seeded.");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Students already exist.");
            //    }
            //}

            //using (StudentPortalContext context = new StudentPortalContext())
            //{
            //    var result = context.Students
            //        .Where(s => s.Gpa > 3.0)
            //        .OrderByDescending(s => s.Gpa)
            //        .Select(s => s.FullName)
            //        .ToList();

            //    Console.WriteLine("Students with GPA > 3.0:");

            //    foreach (var name in result)
            //    {
            //        Console.WriteLine(name);
            //    }
            //}

            //using (StudentPortalContext context = new StudentPortalContext())
            //{
            //    Console.WriteLine($"Student Count = {context.Students.Count()}");
            //    Console.WriteLine($"Average GPA = {context.Students.Average(s => s.Gpa):F2}");
            //}


            /*
context.Students.Where(s => s.Gpa > 3.0).ToList()

This filters the students in SQL Server first,
then retrieves only the matching rows.

context.Students.ToList().Where(s => s.Gpa > 3.0)

This retrieves all students from the database first,
then filters them in memory using C#.

I used the first approach because it is more efficient
and transfers less data from the database.
*/




            /*
Part I - Wrap-Up Reflection

1.
Lab ID = 7

Part C GPA Threshold:
2.5 + ((7 % 4) * 0.3)
= 2.5 + (3 * 0.3)
= 3.4

Part D Instructor Experience:
(7 % 5) + 3
= 2 + 3
= 5 years

Part G Student Property:
7 % 3 = 1
Therefore, I added the CreditsCompleted property to the Student class.

2.
A silently missing join row is more dangerous than a crash because the program continues running without showing an error. 
This can make users believe the returned data is complete,
even though some records are missing. 
A crash is easier to notice and fix, while missing data can lead to incorrect decisions.

3.
Add-Migration and Update-Database are separated for safety. 
Add-Migration only generates the migration file so I can review the changes before they are applied.
Update-Database actually modifies the database. 
 This separation helps catch mistakes before changing or damaging the database.

4.
The LINQ query looks exactly the same when running against a database or a List, 
but the execution is different. When using a database, 
Entity Framework translates the LINQ query into SQL and executes it on SQL Server. 
Deferred execution is more important because the SQL query is not sent to the database until the results are actually needed,
which improves performance and avoids retrieving unnecessary data.
*/



        }
    }







        }
    


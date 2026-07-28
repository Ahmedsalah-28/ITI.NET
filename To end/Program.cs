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

            using (StudentPortalContext context = new StudentPortalContext())
            {
                if (!context.Students.Any())
                {
                    context.Students.AddRange(
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
                        });

                    context.SaveChanges();

                    Console.WriteLine("Students Seeded.");
                }
                else
                {
                    Console.WriteLine("Students already exist.");
                }
            }

            using (StudentPortalContext context = new StudentPortalContext())
            {
                var result = context.Students
                    .Where(s => s.Gpa > 3.0)
                    .OrderByDescending(s => s.Gpa)
                    .Select(s => s.FullName)
                    .ToList();

                Console.WriteLine("Students with GPA > 3.0:");

                foreach (var name in result)
                {
                    Console.WriteLine(name);
                }
            }

            using (StudentPortalContext context = new StudentPortalContext())
            {
                Console.WriteLine($"Student Count = {context.Students.Count()}");
                Console.WriteLine($"Average GPA = {context.Students.Average(s => s.Gpa):F2}");
            }


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
    


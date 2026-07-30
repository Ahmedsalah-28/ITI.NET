


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.ComponentModel;
using System.Text;

namespace ConsoleApp1
{
    public class Program
    {
        static async Task Main(string[] args)
        {


            using (StudentPortalContext context = new StudentPortalContext())
            {
                // ===========================
                // Students
                // ===========================

                var students = new List<Student>
                        {
                            new Student
                            {
                                FullName = "Yara Adel",
                                YearOfStudy = 2,
                                Gpa = 3.5
                            },
                            new Student
                            {
                                FullName = "Omar Hesham",
                                YearOfStudy = 3,
                                Gpa = 2.8
                            },
                            new Student
                            {
                                FullName = "Nada Samir",
                                YearOfStudy = 1,
                                Gpa = 3.9
                            },
                            new Student
                            {
                                FullName = "Kareem Fouad",
                                YearOfStudy = 4,
                                Gpa = 3.2
                            }
                        };

                context.Students.AddRange(students);

                // ===========================
                // Instructors
                // ===========================

                var hamdy = new Instructor
                {
                    FullName = "Hamdy",
                    YearsOfExperience = 10
                };

                var mona = new Instructor
                {
                    FullName = "Mona Khalil",
                    YearsOfExperience = 6
                };

                context.Instructors.AddRange(hamdy, mona);

                await context.SaveChangesAsync();

                // ===========================
                // Courses
                // ===========================

                var courses = new List<Course>
                            {
                                new Course
                                {
                                    CourseName = "Web Development Using .NET",
                                    Credits = 4,
                                    InstructorId = hamdy.Id
                                },
                                new Course
                                {
                                    CourseName = "Database Fundamentals",
                                    Credits = 3,
                                    InstructorId = mona.Id
                                }
                            };

                context.Courses.AddRange(courses);

                await context.SaveChangesAsync();

                Console.WriteLine("Data Inserted Successfully.");
            }



            using (StudentPortalContext context = new StudentPortalContext())
            {

                //-----------------part c -------------------------
                var nada = await context.Students
                    .FirstAsync(s => s.FullName == "Nada Samir");

                Console.WriteLine($"Name: {nada.FullName}");
                Console.WriteLine($"Current GPA: {nada.Gpa}");


                // Lab ID = 7
                // GPA = 3.0

                nada.Gpa = 3.0;

                Console.WriteLine($"New GPA in C#: {nada.Gpa}");

                //// C# shows GPA = 3.0.
                //// SSMS still shows the old GPA because SaveChangesAsync()
                //// has not been called yet.


                await context.SaveChangesAsync();

                Console.WriteLine("Changes saved.");

                //// EF Core tracked the Student entity.
                //// It detected that only the Gpa property changed,
                //// so it generated an UPDATE statement for that property.


                var me = new Student
                {
                    FullName = "Ahmed Salah",
                    YearOfStudy = 2,
                    Gpa = 3.0
                };

                Console.WriteLine($"Before Save Id = {me.Id}");

                await context.Students.AddAsync(me);

                await context.SaveChangesAsync();

                Console.WriteLine($"After Save Id = {me.Id}");

                //// Before Save the Id was 0.
                //// After Save SQL Server generated the identity value.
                //me.YearOfStudy = 3;

                await context.SaveChangesAsync();

                Console.WriteLine("Year updated.");

                context.Students.Remove(me);

                await context.SaveChangesAsync();

                Console.WriteLine("Student deleted.");

                //// Remove() has no Async version because it only marks
                //// the entity as Deleted in the Change Tracker.
                //// The actual database operation happens when
                //// SaveChangesAsync() is called.




                //-----------------part D -------------------------

                //What operation does Up perform on FullName ? (Name it exactly.)
                //AlterColumn


                //What are nullable: and oldNullable: set to, and what does the difference mean?
                //nullable: false
                //oldNullable: false not changed

                //What two kinds of existing row could make this migration fail?
                //FullName IS NULL
                //LEN(FullName) > 100

                //Run a SELECT in SSMS to check whether your table contains either kind.Paste the query and its result count.

                /*

                SELECT *
                FROM Students
                WHERE FullName IS NULL
                   OR LEN(FullName) > 100;

                SELECT COUNT(*) AS InvalidRows
                FROM Students
                WHERE FullName IS NULL
                   OR LEN(FullName) > 100;

                            0


                 */

                try
                {
                    var invalidStudent = new Student
                    {
                        FullName = null!,
                        YearOfStudy = 2,
                        Gpa = 3.0
                    };

                    await context.Students.AddAsync(invalidStudent);

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Database rejected the student because FullName is required.");

                    Console.WriteLine(ex.GetType().Name);
                }

                //// Exception Type: DbUpdateException




                //-----------------part E -------------------------

                // Keeping AssignedCourseName and InstructorId would create
                // two sources of truth for the same relationship.
                // They could become inconsistent.


                /*
                                 Up



                 migrationBuilder.DropForeignKey(
                name: "FK_Courses_Instructors_InstructorId",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Instructors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "InstructorId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Instructors_InstructorId",
                table: "Courses",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
                 */



                //Apply it. In SSMS, confirm the InstructorId column exists and find the foreign-key constraint under the table's Keys node. Record its exact name.
                //FK_Courses_Instructors_InstructorId



                var hamdy = await context.Instructors
                        .FirstAsync(i => i.FullName == "Hamdy");

                var webCourse = await context.Courses
                    .FirstAsync(c => c.CourseName == "Web Development Using .NET");

                webCourse.InstructorId = hamdy.Id;

                await context.SaveChangesAsync();

                Console.WriteLine("Course linked successfully.");
                /* 
                 
   SELECT *
FROM Courses
WHERE CourseName='Web Development Using .NET';
 1	Web Development Using .NET	4	1                
                 */



                try
                {
                    var course = new Course
                    {
                        CourseName = "AI Fundamentals",
                        Credits = 3,
                        InstructorId = 9999
                    };

                    await context.Courses.AddAsync(course);

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Foreign key constraint worked.");

                    Console.WriteLine(ex.InnerException?.Message);
                }

                // Exception: DbUpdateException
                //Constraint: FK_Courses_Instructors_InstructorId
                // AssignedCourseName would have accepted any text, even if no Instructor actually existed.




                //-----------------part F -------------------------

                Lab ID = 7
                 Extra courses required = (7 % 3) + 2 = 3

                var hamdy = await context.Instructors
                    .FirstAsync(i => i.FullName == "Hamdy");

                var extraCourses = new List<Course>()
                                    {
                                        new Course
                                        {
                                            CourseName = "ASP.NET Core",
                                            Credits = 3,
                                            InstructorId = hamdy.Id
                                        },
                                        new Course
                                        {
                                            CourseName = "Entity Framework Core",
                                            Credits = 3,
                                            InstructorId = hamdy.Id
                                        },
                                        new Course
                                        {
                                            CourseName = "REST API Development",
                                            Credits = 3,
                                            InstructorId = hamdy.Id
                                        }
                                    };

                await context.Courses.AddRangeAsync(extraCourses);
                await context.SaveChangesAsync();

                Console.WriteLine("3 extra courses added.");
                // Added 3 extra courses (Lab ID = 7)



                var instructors = await context.Instructors
                                    .ToListAsync();

                foreach (var instructor in instructors)
                {
                    Console.WriteLine($"{instructor.FullName} : {instructor.Courses.Count}");
                }

                /*
Counts printed:
Hamdy : 0
Mona : 0

SQL Queries:
1

Reason:
Courses navigation property was not loaded.
Lazy loading is disabled.
*/



                var instructors = await context.Instructors
                                .Include(i => i.Courses)
                                .ToListAsync();

                foreach (var instructor in instructors)
                {
                    Console.WriteLine(instructor.FullName);

                    foreach (var course in instructor.Courses)
                    {
                        Console.WriteLine($"   {course.CourseName}");
                    }
                }

                /*
SQL Queries:
1

Hamdy now contains all his courses.
Mona contains her courses.

                Hamdy
   Web Development Using .NET
   ASP.NET Core
   Entity Framework Core
   REST API Development
Mona Khalil
   Database Fundamentals

*/


                /*
SQL returns one row for every Instructor-Course pair.
Example:

Hamdy + Web Development Using .NET
Hamdy + ASP.NET Core
Hamdy + Entity Framework Core
Mona + Database Fundamentals

EF Core automatically combines duplicate instructor rows
into one Instructor object and fills its Courses collection.
*/



                var instructor = await context.Instructors
                         .FirstAsync(i => i.FullName == "Hamdy");

                Console.WriteLine($"Before Loading : {instructor.Courses.Count}");

                await context.Entry(instructor)
                    .Collection(i => i.Courses)
                    .LoadAsync();

                Console.WriteLine($"After Loading : {instructor.Courses.Count}");

                /*
Before Load:
0

After Load:
4

Queries:
2

Query 1 -> Instructor
Query 2 -> Courses
*/


                var students = await context.Students
                        .AsNoTracking()
                        .ToListAsync();

                students[0].Gpa = 1.5;

                await context.SaveChangesAsync();

                Console.WriteLine("SaveChanges finished.");


                /*
Nothing changed in the database.

Reason:
AsNoTracking disables change tracking.

EF Core does not know the entity was modified,
so SaveChangesAsync() generates no UPDATE statement.
*/




            }


        }







    }
}

/*
Part A

PreInit reported:
Students = 4

Applied migrations = 2
*/




/*
=========================================================
Part G - Wrap-Up Reflection
=========================================================

Lab ID = 7

Derived Values:

1. Part C GPA
   = 3.0 + ((7 % 7) * 0.1)
   = 3.0 + (0 * 0.1)
   = 3.0

2. Part E Delete Behavior
   = 7 % 2
   = 1
   = DeleteBehavior.SetNull

3. Part F Extra Courses
   = (7 % 3) + 2
   = 1 + 2
   = 3 extra courses


---------------------------------------------------------
Part E - OnDelete Reflection
---------------------------------------------------------

I used DeleteBehavior.SetNull.

This requires InstructorId to be nullable (int?).
When an Instructor is deleted, EF Core automatically
sets InstructorId to NULL instead of deleting the Course.

This preserves the Course records while removing only the
relationship to the deleted Instructor.


---------------------------------------------------------
Migration Reflection
---------------------------------------------------------

No rollback needed.

If a migration had failed, I would have used:

Update-Database <PreviousMigrationName>

Remove-Migration

Then I would fix the model or migration issue,
create a new migration, and apply it again.


---------------------------------------------------------
Multiple Enumeration vs N+1
---------------------------------------------------------

Both problems execute more database queries than necessary.

Multiple Enumeration executes the same query multiple times
because the same IEnumerable is enumerated repeatedly.

N+1 executes one query to load the parent entities and then
one additional query for each parent entity to load its
related data.

In my Part F results:

- Without Include:
  1 query was executed, but the Courses navigation properties
  were not loaded.

- With Include:
  1 query loaded both instructors and their related courses.

- Explicit Loading:
  2 queries were executed:
  one for the Instructor and one for the related Courses.

The common problem is unnecessary database round-trips.
Reducing the number of queries improves application
performance.
*/
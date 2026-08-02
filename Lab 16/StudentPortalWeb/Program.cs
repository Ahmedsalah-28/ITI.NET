// LAB 16 — Lab ID: 7 | MAX_YEAR = 4 | MIN_GPA = 3.0 | INTAKE_CODE = itiB
// The default route is placed at the bottom because it is the most general route.
// If it were placed before more specific routes,
// it could match requests first and prevent the specific routes from being reached.

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentPortalWeb.Constraints;
using StudentPortalWeb.Models;
using System;

namespace StudentPortalWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            

            builder.Services.AddDbContext<StudentPortalContext>(options =>
            {

                options.UseSqlServer("Data Source=AHMEDSALAH\\SQLEXPRESS;Initial Catalog=ITI_StudentPortal;Integrated Security=True;Encrypt=True;Trust Server Certificate=True")
                .LogTo(Console.WriteLine , LogLevel.Information)
                .EnableSensitiveDataLogging();
            });



            builder.Services.AddRouting(options =>
            {
                options.ConstraintMap.Add("honourBand", typeof(HonourBandConstraint));
                options.ConstraintMap.Add("intakecode", typeof(IntakeCodeConstraint));
            });

            var app = builder.Build();
           
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.Use(async (context, next) =>
            {
                Console.WriteLine($"[START] Request path : {context.Request.Path}");
                await next.Invoke();
                Console.WriteLine($"[END] Request path : {context.Request.Path}");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllerRoute(
                name: "studentsList",
                pattern: "students",
                defaults: new { controller = "Students", action = "Index" }
                );
            // Yes, MAX_YEAR is accepted because the range constraint is inclusive.
            app.MapControllerRoute(
                name: "studentsTop",
                pattern: "students/top/{count:int:range(1,4)}",
                defaults: new
                {
                    controller = "Students",
                    action = "Top"
                });



            app.MapControllerRoute(
                name: "studentsDetails",
                pattern: "students/{id:int}",
                defaults: new { controller = "Students", action = "Details" }
                );

            app.MapControllerRoute(
                name: "studentsByYear",
                pattern: "students/year/{year:int:range(1,4)}",
                defaults: new { controller = "Students", action = "ByYear" }
                );

            app.MapControllerRoute(
                name: "studentsHonours",
                pattern: "students/honours/{band:honourBand}",
                defaults: new { controller = "Students", action = "Honours" }
                );
            // Yes. It is acceptable because different URLs can represent the same resource when it improves usability or readability.
            app.MapControllerRoute(
                name: "studentsRoster",
                pattern: "roster",
                defaults: new
                {
                    controller = "Students",
                    action = "Index"
                });

            app.MapControllerRoute(
                name: "studentsIntake",
                pattern: "students/intake/{code:intakecode}",
                defaults: new
                {
                    controller = "Students",
                    action = "Intake"
                });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllers();




            app.Run();
        }
    }
}

#region 📋 Full TODO Checklist
// ---------------------------------------------------------------------
// Program.cs — Phase One (before builder.Build())
//   TODO 1: Register your constraint's nickname in the constraint map
//           [Block 4 — sits first only because it must run before Build]
//
// Program.cs — Phase Two (after builder.Build())
//   TODO 2: Custom routes for the students list and the student detail   [Block 2]
//   TODO 3: Constrain the detail id to integers; add the by-year route   [Block 3]
//   TODO 4: Add the honours route using your own constraint's nickname   [Block 4]
//
// Constraints/HonourBandConstraint.cs
//   TODO 5: Implement Match so only the three real band names pass       [Block 4]
//
// Controllers/StudentsController.cs
//   TODO 6: Give the search action its own address, and read the query   [Block 5]
// ---------------------------------------------------------------------
#endregion

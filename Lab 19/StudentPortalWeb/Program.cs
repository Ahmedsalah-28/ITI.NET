

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

            builder.Services.AddRouting(options =>
            {
                options.ConstraintMap.Add("honourBand", typeof(HonourBandConstraint));
            });

            builder.Services.AddDbContext<StudentPortalContext>(options =>
            {
                options.UseSqlServer("Data Source=AHMEDSALAH\\SQLEXPRESS;Initial Catalog=ITI_StudentPortal2;Integrated Security=True;Encrypt=True;Trust Server Certificate=True")
                .LogTo(Console.WriteLine , LogLevel.Information)
                .EnableSensitiveDataLogging();
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
                defaults: new { controller = "Students", action = "Index" });

            app.MapControllerRoute(
                name: "studentsDetails",
                pattern: "students/{id:int}",
                defaults: new { controller = "Students", action = "Details" });

            app.MapControllerRoute(
                name: "studentsByYear",
                pattern: "students/year/{year:int:range(1,4)}",
                defaults: new { controller = "Students", action = "ByYear" });

            app.MapControllerRoute(
                name: "studentsHonours",
                pattern: "students/honours/{band:honourBand}",
                defaults: new { controller = "Students", action = "Honours" });


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

#region 📋 Full TODO Checklist
// ---------------------------------------------------------------------
// Nothing in Program.cs today.
//
// Models/StudentPortalContext.cs
//   TODO 1: The Enrollment entity — four parts, four locations   [Block 1]
//   TODO 2: Fluent API — two relationships + a unique index      [Block 2]
//
// Controllers/StudentsController.cs
//   TODO 3: Include/ThenInclude on Details                       [Block 3]
//
// Controllers/CoursesController.cs (new file)
//   TODO 4: Field+constructor, Index, Details — three parts      [Block 4]
//
// Views/Students/Details.cshtml
//   TODO 5: Show the enrolled-courses table, reusing <gpa-badge>  [Block 4]
// ---------------------------------------------------------------------
#endregion

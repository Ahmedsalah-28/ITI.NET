//part A
// LAB 17 — Lab ID: 7 | MIN_GPA_EDIT = 2.6 | MAX_YEAR_EDIT = 3
// The GET Create action is not async because it only returns an empty view
// and does not perform any asynchronous database operations.


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
                options.UseSqlServer("Data Source=AHMEDSALAH\\SQLEXPRESS;Initial Catalog=ITI_StudentPortal;Integrated Security=True;Encrypt=True;Trust Server Certificate=True")
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
// Nothing in Program.cs today. Routing finished yesterday.
//
// Controllers/StudentsController.cs
//   TODO 1: One action that answers with four different kinds of result  [Block 1]
//   TODO 2: One action that proves where each parameter came from        [Block 2]
//   TODO 3: The empty form — the GET half of Create                      [Block 3]
//   TODO 4: The POST half of Create, and the attribute that marks it     [Block 3]
//   TODO 6: Refuse to save when the submitted data breaks the rules      [Block 4]
//   TODO 7: Save, then redirect instead of rendering                     [Block 5]
//
// Models/StudentPortalContext.cs
//   TODO 5: Add the validation rules the form will be checked against    [Block 4]
// ---------------------------------------------------------------------
#endregion

// LAB 19 — Lab ID: 7 | MIN_GRADE_LAB = 2.5 | COURSE_COUNT = 3
//
// CoursesController.Index can use Include(c => c.Enrollments) because it only needs
// the Enrollment collection. CoursesController.Details also needs each Enrollment's
// related Student, so it requires ThenInclude(e => e.Student).

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortalWeb.Models;

namespace StudentPortalWeb.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly StudentPortalContext _context;

        public EnrollmentsController(StudentPortalContext context)
        {
            _context = context;
        }
        // This action queries the database because it must load the available Students and Courses
        // to populate the dropdown lists, while the Students Create() GET only displayed an empty form.
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Students"] = await _context.Students
                .OrderBy(s => s.FullName)
                .ToListAsync();

            ViewData["Courses"] = await _context.Courses
                .OrderBy(c => c.CourseName)
                .ToListAsync();

            return View();
        }

        // If validation fails, the dropdown lists must be loaded again because ViewData is not preserved between requests.
        // EnrollmentDate is set in the controller because it represents the actual server-side
        // enrollment time and should not be supplied or modified by the user.


        //part E
        // When I attempted to enroll the same student in the same course twice,
        // SQL Server rejected the duplicate insert because of the unique index,
        // matching the behavior demonstrated in Block 5.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            enrollment.EnrollmentDate = DateTime.Now;
            //Console.WriteLine(ModelState.IsValid);
            if (!ModelState.IsValid)
            {

                //foreach (var item in ModelState)
                //{
                //    Console.WriteLine($"Field: {item.Key}");

                //    foreach (var error in item.Value.Errors)
                //    {
                //        Console.WriteLine($"Error: {error.ErrorMessage}");
                //    }
                //}

                ViewData["Students"] = await _context.Students
                    .OrderBy(s => s.FullName)
                    .ToListAsync();

                ViewData["Courses"] = await _context.Courses
                    .OrderBy(c => c.CourseName)
                    .ToListAsync();

                return View(enrollment);
            }

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Students", new { id = enrollment.StudentId });
        }

    }


}

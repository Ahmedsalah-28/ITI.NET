// LAB 20 — Lab ID: 7 | MIN_GPA_LAB = 2.5 | MAX_YEAR_LAB = 3
//
// (a) Controllers can overload Create() because HTTP attributes distinguish them,
// while Razor Pages uses different handler method names such as OnGet and OnPost.
//
// (b) Using a property allows both handlers on the page model to access the same
// bound Student object.
//
// (c) Returning the page after a successful POST would cause the browser to
// resubmit the form when the user refreshes (F5), creating duplicate records.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentPortalWeb.Models;

namespace StudentPortalWeb.Pages.Roster
{
    public class CreateModel : PageModel
    {
        private readonly StudentPortalContext _context;

        public CreateModel(StudentPortalContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Student Student { get; set; } = new();

        // The empty form needs no data, but an OnGet handler clearly
        // represents the request that displays the page.
        public void OnGet()
        {
        }

        // ==================== Part D ====================
        //
        // Refreshing repeats the GET request after the redirect,
        // not the original POST, so the form is not submitted again
        // and no duplicate row is inserted.
        /////////////////////////////////////////////////////////////////////////
        //                          4
        // Prediction:
        // Without [BindProperty], the posted form values will not be bound to the
        // Student property, so the submitted data will be lost and the student
        // will not be inserted.
        //
        // Observed:
        // The page returned with the default values (Year = 0 and GPA = 0) because
        // the Student property was not bound. The validation message appeared and
        // no new row was inserted into the database.
        /*
         HTTP Status: 200 OK
        Row inserted: No
        Message:
        GPA must be at least 2.5 for this intake.      
         */
        // ================================================

        public async Task<IActionResult> OnPostAsync()
        {
            // Return the same page so the user keeps the entered values
            // and validation messages.
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Student.Gpa < 2.5)
            {
                ModelState.AddModelError(
                    "Student.Gpa",
                    "GPA must be at least 2.5 for this intake.");
            }

            if (Student.YearOfStudy > 3)
            {
                ModelState.AddModelError(
                    "Student.YearOfStudy",
                    "Year of study may not exceed 3 for this intake.");
            }

            // Re-check ModelState after adding custom errors.
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.Students.AddAsync(Student);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{Student.FullName} was added the Razor Pages way";

            return RedirectToPage("./Index");
        }
    }
}
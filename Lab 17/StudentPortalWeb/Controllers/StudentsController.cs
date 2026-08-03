using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortalWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentPortalWeb.Controllers
{
    public class StudentsController : Controller
    {

        private readonly StudentPortalContext _context;

        public StudentsController(StudentPortalContext context)
        {
            _context = context;
        }



        public IActionResult Demo(int id)
        {
            if (id == 0) return NotFound();
            if (id == 1) return Content("I'm a plain text , not a page.");
            if (id == 2) return Json(new { Message = "This is a JSON" , Id = id });
            if (id == 3) return RedirectToAction("Index");
            return View();
        }

       

        public IActionResult Echo(
            [FromRoute] int id,
            [FromQuery] string note,
            [FromHeader(Name = "User-Agent")] string agent
            )
        {
            return Content($"id (route) = {id} | note (query) = {note} | agent (header) = {agent}");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{student.FullName} was Added";

            return RedirectToAction("Index");
        }


        // We load the student from the database so the form is pre-filled with
        // the current values instead of showing an empty form.
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student is null)
            {
                return NotFound();
            }

            return View(student);
        }

        // If the parameter was renamed to studentId but the route still used {id},
        // model binding would not bind the route value automatically because their names no longer match.
        [HttpPost]
        public async Task<IActionResult> Edit(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            _context.Students.Update(student);

            await _context.SaveChangesAsync();

            TempData["Message"] = $"{student.FullName} was updated.";

            //return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(AhmedConfirmed), new { id = student.Id });
        }

        // Error message:
        //GPA must be between 2.6 and 4.0.
        //Database check:
        //The row was not updated.I verified in SSMS that the student's data remained unchanged.



        // We reload the student from the database to ensure the confirmation page
        // displays the saved data instead of the in-memory object from the POST request.
        // The student must be reloaded from the database to display the actual saved data,
        // rather than the in-memory object from the POST request.
        public async Task<IActionResult> AhmedConfirmed(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student is null)
            {
                return NotFound();
            }

            return View("Details", student);
        }
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .OrderBy(s => s.FullName)
                .ToListAsync();

            return View(students);
        }

        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student is null)
            {
                return NotFound();
            }

            return View(student);
        }

        public async Task<IActionResult> ByYear(int year)
        {
            var students = await _context.Students
                .Where(s => s.YearOfStudy == year)
                .OrderBy(s => s.FullName)
                .ToListAsync();

            ViewData["Year"] = year;

            return View(students);
        }

        public async Task<IActionResult> Honours(string band)
        {
            if (string.IsNullOrWhiteSpace(band))
            {
                return NotFound();
            }

            IQueryable<Student> query = _context.Students;

            if (string.Equals(band, "first", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(s => s.Gpa >= 3.5);
            }
            else if (string.Equals(band, "second", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(s => s.Gpa >= 3.0 && s.Gpa < 3.5);
            }
            else
            {
                query = query.Where(s => s.Gpa < 3.0);
            }

            var students = await query
                .OrderBy(s => s.FullName)
                .ToListAsync();

            ViewData["Band"] = band.ToLowerInvariant();

            return View(students);
        }

        
        [Route("students/search")]
        public async Task<IActionResult> Searching([FromQuery] string name)
        {
            IQueryable<Student> query = _context.Students;

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(s => s.FullName.Contains(name));
            }

            var students = await query
                .OrderBy(s => s.FullName)
                .ToListAsync();

            ViewData["Name"] = name;

            return View(students);
        }

        
    }
}



using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortalWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentPortalWeb.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentPortalContext _context;

        public StudentsController(StudentPortalContext context)
        {
            _context = context;
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


        [HttpGet("students/top/{count:int:range(1,4)}")]
        public async Task<IActionResult> Top([FromRoute] int count)
        {
            var students = await _context.Students
                .OrderByDescending(s => s.Gpa)
                .Take(count)
                .ToListAsync();

            return View("Index", students);
        }

        [HttpGet("students/intake/{code:intakecode}")]

        public async Task<IActionResult> Intake(string code)
        {
            var students = await _context.Students
                .OrderBy(s => s.FullName)
                .ToListAsync();

            return View("Index", students);
        }


        // /Students/About is 404 because this action uses attribute routing only,
        // not the default conventional route.
        // minGpa belongs in the query string because it is a filter value,
        // not part of the resource identity.

        [Route("about/ahmed")]
        // /Students/About returns 404 because this action is reachable only through
        // its attribute route and not through the default conventional route.
        // minGpa is placed in the query string because it is a filtering option,
        // not part of the resource identity.

        public async Task<IActionResult> About([FromQuery] double minGpa = 3.0)
        {
            var students = await _context.Students
                .Where(s => s.Gpa >= minGpa)
                .OrderBy(s => s.FullName)
                .ToListAsync();

            return View("Index", students);
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

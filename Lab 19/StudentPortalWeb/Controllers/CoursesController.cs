

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortalWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StudentPortalWeb.Controllers
{
    public class CoursesController : Controller
    {


        private readonly StudentPortalContext _context;

        public CoursesController(StudentPortalContext context)
        {
            _context = context;
        }

      
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .ToListAsync();
            return View(courses);
        }
        [HttpGet("/courses/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course is null)
                return NotFound();

            return View(course);
        }
    }
}

using Lab15_StudentPortalWeb.Models;
using Lab15_StudentPortalWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab15_StudentPortalWeb.Controllers;

public class HomeController : Controller
{
    private readonly StudentPortalContext _context;
    private readonly IAhmedStampService _stampA;
    private readonly IAhmedStampService _stampB;

    public HomeController(
        StudentPortalContext context,
        IAhmedStampService stampA,
        IAhmedStampService stampB)
    {
        _context = context;
        _stampA = stampA;
        _stampB = stampB;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Owner = _stampA.Owner;
        ViewBag.StampA = _stampA.Stamp;
        ViewBag.StampB = _stampB.Stamp;

        var students = await _context.Students
            .OrderBy(s => s.FullName)
            .ToListAsync();

        return View(students);
    }
}
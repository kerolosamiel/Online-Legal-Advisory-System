using ELawyer.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ELawyer.Areas.Specialization.Controllers;

public class SpecializationController : Controller
{
    private readonly ApplicationDbContext _context;

    public SpecializationController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /api/specialization
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var specializations = await _context.Specializations.Select(s => s).ToListAsync();
        return Ok(specializations);
    }
}
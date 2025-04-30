using Microsoft.AspNetCore.Mvc;

namespace ELawyer.Areas.Specialization;

public class SpecializationController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
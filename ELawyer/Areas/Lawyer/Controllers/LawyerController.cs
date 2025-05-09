using System.Security.Claims;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using ELawyer.Models.ViewModels;
using ELawyer.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELawyer.Areas.Lawyer.Controllers;

public class LawyerController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext dbContext;

    public LawyerController(ApplicationDbContext dbContext, IUnitOfWork unitOfWork,
        IWebHostEnvironment webHostEnvironment)
    {
        this.dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    public IWebHostEnvironment _webHostEnvironment { get; }

    public IActionResult Index()
    {
        var lawyerList = _unitOfWork.Lawyer
            .GetAll(includeproperties: "LawyerSpecializations.Specialization").OrderByDescending(l => l.AverageRateing)
            .ToList();


        return View(lawyerList);
    }

    public IActionResult Details(int? id)

    {
        if (User.IsInRole(SD.ClientRole))
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Client = _unitOfWork.Client.Get(l => l.Id == user.Client.Id);

            var paymentfromdb =
                _unitOfWork.Payment.Get(p => p.ClientId == Client.Id && p.PaidAt == null, "Lawyer,Client");
            if (paymentfromdb != null) ViewBag.pay = 0;
            ViewData["ClientId"] = Client.Id;
        }


        var Lawyer = _unitOfWork.Lawyer.Get(l => l.Id == id,
            "specializationnews.Specialization,specializationnews.Specialization.SubSpecializations,Service");
        var Rating = _unitOfWork.Rating.GetAll(r => r.LawyerId == id, "Client");
        var ratings = _unitOfWork.Rating.GetAll(r => r.LawyerId == id);


        if (ratings == null)
            Lawyer.AverageRateing = 0;
        else
            Lawyer.AverageRateing = ratings.Average(r => r.Rate);
        var clientLawyerRating = new ClientLawyerRating
        {
            Lawyer = Lawyer,
            Rating = Rating
        };
        _unitOfWork.Lawyer.Update(Lawyer);
        _unitOfWork.Save();

        return View(clientLawyerRating);
    }


    public IActionResult Edit()
    {
        var SpecializationList = _unitOfWork.Specializationnew.GetAll().Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.ID.ToString()
        }).ToList();

        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Lawyer = _unitOfWork.Lawyer.Get(l => l.Id == user.Lawyer.Id);

        var lawyerEdit = new LawyerEditViewModel
        {
            Lawyer = Lawyer,
            SpecializationList = SpecializationList
        };
        return View(lawyerEdit);
    }

    [HttpPost]
    public IActionResult Edit(LawyerEditViewModel newLawyer, IFormFile? file, IFormFile? file1, IFormFile? file2)
    {
        var oldLawyer = _unitOfWork.Lawyer.Get(i => i.Id == newLawyer.Lawyer.Id, "specializationnews.Specialization");
        if (oldLawyer == null) return NotFound();

        if (ModelState.IsValid)
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;

            if (file != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Profile");


                if (!string.IsNullOrEmpty(oldLawyer.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, oldLawyer.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                }


                using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create,
                           FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                {
                    file.CopyTo(fileStream);
                }


                newLawyer.Lawyer.ImageUrl = @"images\Lawyer\Profile\" + fileName;
            }
            else
            {
                newLawyer.Lawyer.ImageUrl = oldLawyer.ImageUrl;
            }


            if (file1 != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file1.FileName);
                var lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Cards");


                if (!string.IsNullOrEmpty(oldLawyer.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, oldLawyer.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                }


                using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create,
                           FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                {
                    file1.CopyTo(fileStream);
                }


                oldLawyer.FrontCardImage = @"images\Lawyer\Cards\" + fileName;
            }
            else
            {
                oldLawyer.FrontCardImage = newLawyer.Lawyer.FrontCardImage;
            }

            if (file2 != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file2.FileName);
                var lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Cards");


                if (!string.IsNullOrEmpty(oldLawyer.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, oldLawyer.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                }


                using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create,
                           FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                {
                    file2.CopyTo(fileStream);
                }


                oldLawyer.BackCardImage = @"images\Lawyer\Cards\" + fileName;
            }
            else
            {
                oldLawyer.BackCardImage = newLawyer.Lawyer.BackCardImage;
            }


            oldLawyer.LawyerSpecializations = newLawyer.SelectedSpecialization
                .Select(d => new LawyerSpecialization { SpecializationId = d }).ToList();


            _unitOfWork.LawyerSpecialization.RemoveRange(
                _unitOfWork.LawyerSpecialization.GetAll()
                    .Where(ls => ls.LawyerId == oldLawyer.Id)
            );


            foreach (var specId in newLawyer.SelectedSpecialization.Distinct())
            {
                _unitOfWork.LawyerSpecialization.Add(new LawyerSpecialization
                {
                    LawyerId = oldLawyer.Id,
                    SpecializationId = specId
                });
                _unitOfWork.Save();
            }


            _unitOfWork.Lawyer.Update(newLawyer.Lawyer);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }


        return View(newLawyer);
    }


    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var LawyerFromDb = _unitOfWork.Lawyer.Get(x => x.Id == id);

        if (LawyerFromDb == null)
            return NotFound();

        return View(LawyerFromDb);
    }

    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var LawyerFromDb = _unitOfWork.Lawyer.Get(x => x.Id == id);


        if (LawyerFromDb == null)
            return NotFound();

        var imageToBeDeleted = LawyerFromDb.ImageUrl;

        var ImagePath =
            Path.Combine(_webHostEnvironment.WebRootPath,
                imageToBeDeleted.TrimStart('\\'));

        if (System.IO.File.Exists(ImagePath)) System.IO.File.Delete(ImagePath);

        var user = _unitOfWork.ApplicationUser.Get(a => a.Lawyer.Id == id);
        _unitOfWork.ApplicationUser.Remove(user);
        _unitOfWork.Lawyer.Remove(LawyerFromDb);
        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult AddService()
    {
        var service = new Service();
        return View(service);
    }

    [HttpPost]
    public IActionResult AddService(Service service)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Lawyer = _unitOfWork.Lawyer.Get(l => l.Id == user.Lawyer.Id);
        if (ModelState.IsValid && Lawyer.UserStatus == SD.UserStatusVerfied)
        {
            var newservice = new Service();
            newservice.Title = service.Title;
            newservice.Description = service.Description;

            newservice.Status = service.Status;
            newservice.ServiceType = service.ServiceType;
            newservice.CreatedAt = DateTime.Now;
            newservice.Duration = service.Duration;
            newservice.LawyerId = Lawyer.Id;

            _unitOfWork.Service.Add(newservice);

            _unitOfWork.Save();
            Lawyer.ServiceId = newservice.Id;
            _unitOfWork.Lawyer.Update(Lawyer);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        return View(service);
    }

    public IActionResult UpdateService()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Lawyer = _unitOfWork.Lawyer.Get(l => l.Id == user.Lawyer.Id);
        var services = _unitOfWork.Service.Get(s => s.Id == Lawyer.ServiceId);

        return View(services);
    }

    [HttpPost]
    public IActionResult UpdateService(Service service)
    {
        if (ModelState.IsValid)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Lawyer = _unitOfWork.Lawyer.Get(l => l.Id == user.Lawyer.Id);
            var newservice = _unitOfWork.Service.Get(s => s.Id == Lawyer.ServiceId);
            newservice = service;
            _unitOfWork.Service.Update(newservice);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        return View(service);
    }


    public IActionResult DeleteService()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Lawyer = _unitOfWork.Lawyer.Get(l => l.Id == user.Lawyer.Id);


        var serviceFromDb = _unitOfWork.Service.Get(x => x.Id == Lawyer.ServiceId);


        if (serviceFromDb == null)
            return NotFound();

        return View(serviceFromDb);
    }

    [HttpPost]
    public IActionResult DeletePostService()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Lawyer = _unitOfWork.Lawyer.Get(l => l.Id == user.Lawyer.Id);


        var serviceFromDb = _unitOfWork.Service.Get(x => x.Id == Lawyer.ServiceId);


        if (serviceFromDb == null)
            return NotFound();


        Lawyer.ServiceId = null;
        _unitOfWork.Lawyer.Update(Lawyer);
        _unitOfWork.Service.Remove(serviceFromDb);

        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Search()
    {
        var SpecializationList = _unitOfWork.Specializationnew.GetAll().ToList();


        return View(SpecializationList);
    }

    [HttpPost]
    public IActionResult SearchPost(Models.Specialization specialization)
    {
        var lawyerspecialization = _unitOfWork.LawyerSpecialization.GetAll(l => l.SpecializationId == specialization.ID)
            .ToList();
        var lawyers = new List<Models.Lawyer>();
        foreach (var item in lawyerspecialization)
        {
            var lawyer = _unitOfWork.Lawyer.Get(l => l.Id == item.LawyerId, "specializationnews.Specialization");

            if (lawyer != null) lawyers.Add(lawyer);
        }

        if (lawyers != null)
        {
            var lawyerdesc = lawyers.OrderByDescending(l => l.AverageRateing).ToList();
            return View(lawyerdesc);
        }

        return NotFound();
    }
}
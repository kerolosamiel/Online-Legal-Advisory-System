using System.Security.Claims;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using ELawyer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ELawyer.Areas.Client.Controllers;

[Authorize(Roles = SD.ClientRole)]
public class ClientController : Controller
{
    private readonly IEmailSender _emailSender;
    private readonly IUnitOfWork _unitOfWork;

    public ClientController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        _emailSender = emailSender;
    }

    public IWebHostEnvironment _webHostEnvironment { get; }

    public IActionResult Index()
    {
        var lawyerList = _unitOfWork.Lawyer
            .GetAll(l => true, "ApplicationUser")
            .ToList();
        return View(lawyerList);
    }

    public IActionResult Details(int? id)
    {
        var client = _unitOfWork.Client.Get(l => l.Id == id);
        return View(client);
    }

    public IActionResult Edit()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Client = _unitOfWork.Client.Get(l => l.Id == user.Client.Id);
        return View(Client);
    }

    [HttpPost]
    public IActionResult Edit(Models.Client newClient, IFormFile? file)
    {
        var oldClient = _unitOfWork.Client.Get(i => i.Id == newClient.Id);
        if (oldClient == null) return NotFound();

        if (ModelState.IsValid)
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;

            if (file != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var ClientImagePath = Path.Combine(wwwRootPath, @"images\Client\Profile");


                if (!string.IsNullOrEmpty(oldClient.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, oldClient.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                }


                using (var fileStream = new FileStream(Path.Combine(ClientImagePath, fileName), FileMode.Create,
                           FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                {
                    file.CopyTo(fileStream);
                }

                newClient.ImageUrl = @"images\Client\Profile\" + fileName;
            }
            else
            {
                newClient.ImageUrl = oldClient.ImageUrl;
            }


            _unitOfWork.Client.Update(newClient);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }


        return View(newClient);
    }


    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var ClientFromDb = _unitOfWork.Client.Get(x => x.Id == id);


        if (ClientFromDb == null)
            return NotFound();

        return View(ClientFromDb);
    }

    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        var ClientFromDb = _unitOfWork.Client.Get(x => x.Id == id);


        if (ClientFromDb == null)
            return NotFound();

        var imageToBeDeleted = ClientFromDb.ImageUrl;

        var ImagePath =
            Path.Combine(_webHostEnvironment.WebRootPath,
                imageToBeDeleted.TrimStart('\\'));

        if (System.IO.File.Exists(ImagePath)) System.IO.File.Delete(ImagePath);

        var user = _unitOfWork.ApplicationUser.Get(a => a.Client.Id == id);
        _unitOfWork.ApplicationUser.Remove(user);
        _unitOfWork.Client.Remove(ClientFromDb);
        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }


    public IActionResult Rating(int id)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Client = _unitOfWork.Client.Get(l => l.Id == user.Client.Id);
        var Lawyer = _unitOfWork.Lawyer.Get(l => l.Id == id);
        var rating = new Rating
        {
            LawyerId = Lawyer.Id,
            ClientId = user.Client.Id
        };
        return View(rating);
    }

    [HttpPost]
    public IActionResult RatingPost(Rating rating, int id)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Client = _unitOfWork.Client.Get(l => l.Id == user.Client.Id);
        var Lawyer = _unitOfWork.Lawyer.Get(l => l.Id == id);
        var newrating = new Rating();

        newrating.Comment = rating.Comment;
        newrating.Rate = rating.Rate;
        newrating.CreatedAt = DateTime.Now;
        newrating.ClientId = Client.Id;
        newrating.LawyerId = Lawyer.Id;

        _unitOfWork.Rating.Add(newrating);
        _unitOfWork.Save();


        _emailSender.SendEmailAsync(user.Email, "New Rate- ELawyer",
            "<p>you have been rated by client.FirstName  </p>");


        return RedirectToAction("Index");
    }

    public IActionResult EditRating(int id)
    {
        var rating = _unitOfWork.Rating.Get(r => r.ID == id);
        return View(rating);
    }

    [HttpPost]
    public IActionResult EditRatingPost(Rating rating, int id)
    {
        var oldrating = _unitOfWork.Rating.Get(r => r.ID == rating.ID);
        if (oldrating == null) return NotFound();
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Client = _unitOfWork.Client.Get(l => l.Id == user.Client.Id);


        var ratinglawyer = _unitOfWork.Rating.Get(l => l.ID == rating.ID);
        rating.ClientId = Client.Id;
        rating.LawyerId = ratinglawyer.LawyerId;
        if (ModelState.IsValid)
        {
            _unitOfWork.Rating.Update(rating);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }


        return View(rating);
    }

    public IActionResult DeleteRating(int id)
    {
        var rating = _unitOfWork.Rating.Get(r => r.ID == id);
        if (rating == null) return NotFound();
        _unitOfWork.Rating.Remove(rating);
        _unitOfWork.Save();
        return RedirectToAction("Index");
    }
}
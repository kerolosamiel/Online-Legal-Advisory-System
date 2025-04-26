using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ELawyer.Areas.Client.Controllers
{
    public class ClientController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        public IWebHostEnvironment _webHostEnvironment { get; }
        public ClientController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {

            List<ELawyer.Models.Client> ClientList = _unitOfWork.Client.GetAll().ToList();
            return View(ClientList);
        }

        public IActionResult Details(int? id)
        {


            var client = _unitOfWork.Client.Get(l => l.ID == id);
            return View(client);

        }

        public IActionResult Edit()
        {


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Client = _unitOfWork.Client.Get(l => l.ID == user.ClientID);
            return View(Client);





        }
        [HttpPost]
        public IActionResult Edit(ELawyer.Models.Client newClient, IFormFile? file, IFormFile? file1, IFormFile? file2)
        {





            var oldClient = _unitOfWork.Client.Get(i => i.ID == newClient.ID);
            if (oldClient == null)
            {
                return NotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {

                 
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string ClientImagePath = Path.Combine(wwwRootPath, @"images\Client\Profile");


                        if (!string.IsNullOrEmpty(oldClient.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, oldClient.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        using (var fileStream = new FileStream(Path.Combine(ClientImagePath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                        {
                            file.CopyTo(fileStream);
                        }


                        newClient.ImageUrl = @"images\Client\Profile\" + fileName;
                    }
                    else
                    {

                        newClient.ImageUrl = oldClient.ImageUrl;
                    }


                    if (file1 != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file1.FileName);
                        string ClientImagePath = Path.Combine(wwwRootPath, @"images\Client\Cards");


                        if (!string.IsNullOrEmpty(oldClient.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, oldClient.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        using (var fileStream = new FileStream(Path.Combine(ClientImagePath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                        {
                            file1.CopyTo(fileStream);
                        }


                        newClient.FrontCardImage = @"images\Client\Cards\" + fileName;
                    }
                    else
                    {

                        newClient.FrontCardImage = oldClient.FrontCardImage;
                    }
                    if (file2 != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file2.FileName);
                        string ClientImagePath = Path.Combine(wwwRootPath, @"images\Client\Cards");


                        if (!string.IsNullOrEmpty(oldClient.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, oldClient.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        using (var fileStream = new FileStream(Path.Combine(ClientImagePath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                        {
                            file2.CopyTo(fileStream);
                        }


                        newClient.BackCardImage = @"images\Client\Cards\" + fileName;
                    }
                    else
                    {

                        newClient.BackCardImage = oldClient.BackCardImage;
                    }


                    _unitOfWork.Client.Update(newClient);
                    _unitOfWork.Save();

                    return RedirectToAction("Index");
                }
            }


            return View(newClient);
        }


        public IActionResult Delete(int? id)
        {
            
            if (id == null || id == 0)
                return NotFound();

            ELawyer.Models.Client? ClientFromDb = _unitOfWork.Client.Get(x => x.ID == id);

            
            if (ClientFromDb == null)
                return NotFound();

            return View(ClientFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            ELawyer.Models.Client? ClientFromDb = _unitOfWork.Client.Get(x => x.ID == id);

           
            if (ClientFromDb == null)
                return NotFound();

            var imageToBeDeleted = ClientFromDb.ImageUrl;

            var ImagePath =
                                  Path.Combine(_webHostEnvironment.WebRootPath,
                                  imageToBeDeleted.TrimStart('\\'));

            if (System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }

            var user = _unitOfWork.ApplicationUser.Get(a => a.ClientID == id);
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
            var Client = _unitOfWork.Client.Get(l => l.ID == user.ClientID);
            var Lawyer = _unitOfWork.Lawyer.Get(l => l.ID == id);
            Rating rating = new Rating()
            {
                lawyerID = Lawyer.ID,
                ClientID = user.ClientID,


            };
            return View(rating);
        }
        [HttpPost]
        public IActionResult RatingPost(Rating rating, int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Client = _unitOfWork.Client.Get(l => l.ID == user.ClientID);
            var Lawyer = _unitOfWork.Lawyer.Get(l => l.ID == id);
            Rating newrating = new Rating();

            newrating.Comment = rating.Comment;
            newrating.Rate = rating.Rate;
            newrating.CreatedAt = DateTime.Now;
            newrating.ClientID = Client.ID;
            newrating.lawyerID = Lawyer.ID;
            
            _unitOfWork.Rating.Add(newrating);
            _unitOfWork.Save();
          


            _emailSender.SendEmailAsync(user.Email, "New Rate- ELawyer", "<p>you have been rated by client.FirstName  </p>");



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
            if (oldrating == null)
            {
                return NotFound();
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Client = _unitOfWork.Client.Get(l => l.ID == user.ClientID);

            
            var ratinglawyer = _unitOfWork.Rating.Get(l => l.ID == rating.ID);
            rating.ClientID = Client.ID;
            rating.lawyerID = ratinglawyer.lawyerID;
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
            if (rating == null)
            {
                return NotFound();
            }
            _unitOfWork.Rating.Remove(rating);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

    }
}

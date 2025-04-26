using Azure.Core.GeoJson;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using ELawyer.Models.ViewModels;
using ELawyer.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;

namespace ELawyer.Areas.Lawyer.Controllers
{
    public class LawyerController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public IWebHostEnvironment _webHostEnvironment { get; }
        public LawyerController(ApplicationDbContext dbContext, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            List<ELawyer.Models.Lawyer> lawyerList = _unitOfWork.Lawyer.GetAll(includeproperties: "specializationnews.Specialization").OrderByDescending(l => l.AverageRateing).ToList();


            return View(lawyerList);
        }
        public IActionResult Details(int? id)

        {
            if (User.IsInRole(SD.ClientRole))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                var Client = _unitOfWork.Client.Get(l => l.ID == user.ClientID);

                var paymentfromdb = _unitOfWork.Payment.Get(p => p.ClientID == Client.ID && p.PaidAt == null, includeproperties: "Lawyer,Client");
                if (paymentfromdb != null)
                {
                    ViewBag.pay = 0;
                }
                ViewData["ClientId"] = Client.ID;
            }
   

            

            var Lawyer = _unitOfWork.Lawyer.Get(l => l.ID == id, includeproperties: "specializationnews.Specialization,specializationnews.Specialization.SubSpecializations,Service");
            var Rating = _unitOfWork.Rating.GetAll(r => r.lawyerID == id, includeproperties: "Client");
            var ratings = _unitOfWork.Rating.GetAll(r => r.lawyerID == id);

           

            if (ratings == null)
            {
                Lawyer.AverageRateing = 0;
            }
            else
            {
                Lawyer.AverageRateing = ratings.Average(r => r.Rate);
            }
            ClientLawyerRating clientLawyerRating = new ClientLawyerRating()
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
            var SpecializationList = _unitOfWork.specializationnew.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.ID.ToString()
            }).ToList();

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Lawyer = _unitOfWork.Lawyer.Get(l => l.ID == user.LawyerID);

            LawyerEditViewModel lawyerEdit = new LawyerEditViewModel()
            {
                Lawyer = Lawyer,
                SpecializationList = SpecializationList
            };
            return View(lawyerEdit);





        }
        [HttpPost]
        public IActionResult Edit(ELawyer.Models.ViewModels.LawyerEditViewModel newLawyer, IFormFile? file, IFormFile? file1, IFormFile? file2)
        {





            var oldLawyer = _unitOfWork.Lawyer.Get(i => i.ID == newLawyer.Lawyer.ID, includeproperties: "specializationnews.Specialization");
            if (oldLawyer == null)
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
                        string lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Profile");


                        if (!string.IsNullOrEmpty(oldLawyer.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, oldLawyer.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
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
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file1.FileName);
                        string lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Cards");


                        if (!string.IsNullOrEmpty(oldLawyer.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, oldLawyer.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
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
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file2.FileName);
                        string lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Cards");


                        if (!string.IsNullOrEmpty(oldLawyer.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, oldLawyer.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                        {
                            file2.CopyTo(fileStream);
                        }


                        oldLawyer.BackCardImage = @"images\Lawyer\Cards\" + fileName;
                    }
                    else
                    {

                        oldLawyer.BackCardImage = newLawyer.Lawyer.BackCardImage;
                    }


                  
                    oldLawyer.specializationnews = newLawyer.SelectedSpecialization.Select(d => new LawyerSpecialization { SpecializationId = d }).ToList();
                  

                   
                    _unitOfWork.lawyerSpecialization.RemoveRange(
                        _unitOfWork.lawyerSpecialization.GetAll()
                            .Where(ls => ls.LawyerId == oldLawyer.ID)
                    );

                   
                    foreach (var specId in newLawyer.SelectedSpecialization.Distinct())
                    {
                        _unitOfWork.lawyerSpecialization.Add(new LawyerSpecialization
                        {
                            LawyerId = oldLawyer.ID,
                            SpecializationId = specId
                        });
                        _unitOfWork.Save();
                    }



                    _unitOfWork.Lawyer.Update(newLawyer.Lawyer);
                    _unitOfWork.Save();

                    return RedirectToAction("Index");
                }
            }


            return View(newLawyer);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            ELawyer.Models.Lawyer? LawyerFromDb = _unitOfWork.Lawyer.Get(x => x.ID == id);

            if (LawyerFromDb == null)
                return NotFound();

            return View(LawyerFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            ELawyer.Models.Lawyer? LawyerFromDb = _unitOfWork.Lawyer.Get(x => x.ID == id);

           
            if (LawyerFromDb == null)
                return NotFound();

            var imageToBeDeleted = LawyerFromDb.ImageUrl;

            var ImagePath =
                                  Path.Combine(_webHostEnvironment.WebRootPath,
                                  imageToBeDeleted.TrimStart('\\'));

            if (System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }

            var user = _unitOfWork.ApplicationUser.Get(a => a.LawyerID == id);
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
            var Lawyer = _unitOfWork.Lawyer.Get(l => l.ID == user.LawyerID);
            if (ModelState.IsValid && Lawyer.UserStatus == SD.UserStatusVerfied)
            {


                Service newservice = new Service();
                newservice.Title = service.Title;
                newservice.Description = service.Description;
               
                newservice.Status = service.Status;
                newservice.ServiceType = service.ServiceType;
                newservice.CreatedAt = DateTime.Now;
                newservice.Duration = service.Duration;
                newservice.LawyerID = Lawyer.ID;

                _unitOfWork.service.Add(newservice);

                _unitOfWork.Save();
                Lawyer.ServiceID = newservice.ID;
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
            var Lawyer = _unitOfWork.Lawyer.Get(l => l.ID == user.LawyerID);
            var services = _unitOfWork.service.Get(s => s.ID == Lawyer.ServiceID);

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
                var Lawyer = _unitOfWork.Lawyer.Get(l => l.ID == user.LawyerID);
                var newservice = _unitOfWork.service.Get(s => s.ID == Lawyer.ServiceID);
                newservice = service;
                _unitOfWork.service.Update(newservice);
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
            var Lawyer = _unitOfWork.Lawyer.Get(l => l.ID == user.LawyerID);
           

            ELawyer.Models.Service? serviceFromDb = _unitOfWork.service.Get(x => x.ID == Lawyer.ServiceID);

           
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
            var Lawyer = _unitOfWork.Lawyer.Get(l => l.ID == user.LawyerID);
           

            ELawyer.Models.Service? serviceFromDb = _unitOfWork.service.Get(x => x.ID == Lawyer.ServiceID);

          
            if (serviceFromDb == null)
                return NotFound();



            Lawyer.ServiceID = null;
            _unitOfWork.Lawyer.Update(Lawyer);
            _unitOfWork.service.Remove(serviceFromDb);

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));


        }

        public IActionResult Search()
        {
            var SpecializationList = _unitOfWork.specializationnew.GetAll().ToList();

           
            return View(SpecializationList);
        }
        [HttpPost]
        public IActionResult SearchPost(Specializationnew specialization)
        {
            var lawyerspecialization = _unitOfWork.lawyerSpecialization.GetAll(l=>l.SpecializationId == specialization.ID).ToList();
            List<ELawyer.Models.Lawyer> lawyers = new List<Models.Lawyer>();
            foreach (var item in lawyerspecialization)
            {
                var lawyer = _unitOfWork.Lawyer.Get(l => l.ID == item.LawyerId, includeproperties: "specializationnews.Specialization");

                if (lawyer != null)
                {
                   
                    lawyers.Add(lawyer); 
                }
            }
            if (lawyers !=null)
            {
                var lawyerdesc = lawyers.OrderByDescending(l => l.AverageRateing).ToList();
                return View(lawyerdesc);
            }
            return NotFound();



        }
    }
}








using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using ELawyer.Models.ViewModels;
using ELawyer.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ELawyer.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        public IWebHostEnvironment _webHostEnvironment { get; }
        public AdminController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {

            List<ELawyer.Models.Admin> AdminList = _unitOfWork.Admin.GetAll().ToList();


            return View(AdminList);
        }

        public IActionResult Details(int? id)
        {


            var Admin = _unitOfWork.Admin.Get(l => l.ID == id);
            return View(Admin);

        }

        public IActionResult Edit()
        {


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            var Admin = _unitOfWork.Admin.Get(l => l.ID == user.AdminID);
            return View(Admin);





        }
        [HttpPost]
        public IActionResult Edit(ELawyer.Models.Admin newadmin, IFormFile? file)
        {





            var oldAdmin = _unitOfWork.Admin.Get(i => i.ID == newadmin.ID);
            if (oldAdmin == null)
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
                        string ClientImagePath = Path.Combine(wwwRootPath, @"images\Admin\Profile");


                        if (!string.IsNullOrEmpty(oldAdmin.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, oldAdmin.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        using (var fileStream = new FileStream(Path.Combine(ClientImagePath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                        {
                            file.CopyTo(fileStream);
                        }


                        newadmin.ImageUrl = @"images\Admin\Profile\" + fileName;
                    }
                    else
                    {

                        newadmin.ImageUrl = oldAdmin.ImageUrl;
                    }




                    _unitOfWork.Admin.Update(newadmin);
                    _unitOfWork.Save();

                    return RedirectToAction("Index");

                }
            }


            return View(newadmin);
        }
        public IActionResult Confirmation()
        {
         

            ClientLawyerConfirmation viewmodel = new ClientLawyerConfirmation();
           
            




            return View(viewmodel);
        }
        [HttpPost]
        public IActionResult Confirmation(ClientLawyerConfirmation viewmodel, IFormFile? file1, IFormFile? file2)
        {
            if (ModelState.IsValid)
            {

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                

                if (user.Role == SD.LawyerRole)
                {
                    var lawyer = _unitOfWork.Lawyer.Get(c => c.ID == user.LawyerID);
                   viewmodel.Lawyer = lawyer;

                    string wwwRootPath = _webHostEnvironment.WebRootPath;







                    if (file1 != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file1.FileName);
                        string lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Cards");


                        if (!string.IsNullOrEmpty(lawyer.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, lawyer.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                        {
                            file1.CopyTo(fileStream);
                        }


                        viewmodel.Lawyer.FrontCardImage = @"images\Lawyer\Cards\" + fileName;
                    }
                    else
                    {

                        viewmodel.Lawyer.FrontCardImage = lawyer.FrontCardImage;
                    }
                    if (file2 != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file2.FileName);
                        string lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Cards");


                        if (!string.IsNullOrEmpty(lawyer.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, lawyer.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                        {
                            file2.CopyTo(fileStream);
                        }


                        viewmodel.Lawyer.BackCardImage = @"images\Lawyer\Cards\" + fileName;
                    }
                    else
                    {

                        viewmodel.Lawyer.BackCardImage = lawyer.BackCardImage;
                    }
                    lawyer.UserStatus = SD.UserStatusPending;
                    _unitOfWork.Lawyer.Update(viewmodel.Lawyer);

                    _unitOfWork.Save();

                    var Admins = _unitOfWork.Admin.GetAll();
                    foreach (var item in Admins)
                    {
                       
                            var admin = _unitOfWork.ApplicationUser.Get(u => u.AdminID == item.ID);
                        if (admin != null)
                            _emailSender.SendEmailAsync(admin.Email, "new identification- ELawyer", "<p>you have new lawyer want to identification  </p>");
                    }

                    return RedirectToAction("Index");
                    
                }

                else if (user.Role == SD.ClientRole)
                {
                    var client = _unitOfWork.Client.Get(c => c.ID == user.ClientID);
                    viewmodel.Client = client;

                    string wwwRootPath = _webHostEnvironment.WebRootPath;







                    if (file1 != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file1.FileName);
                        string lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Cards");


                        if (!string.IsNullOrEmpty(client.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, client.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                        {
                            file1.CopyTo(fileStream);
                        }


                        viewmodel.Client.FrontCardImage = @"images\Lawyer\Cards\" + fileName;
                    }
                    else
                    {

                        viewmodel.Client.FrontCardImage = client.FrontCardImage;
                    }
                    if (file2 != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file2.FileName);
                        string lawyerImagePath = Path.Combine(wwwRootPath, @"images\Client\Cards");


                        if (!string.IsNullOrEmpty(client.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, client.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
                        {
                            file2.CopyTo(fileStream);
                        }


                        viewmodel.Client.BackCardImage = @"images\Client\Cards\" + fileName;
                    }
                    else
                    {

                        viewmodel.Client.BackCardImage = client.BackCardImage;
                    }
                    client.UserStatus = SD.UserStatusPending;
                    _unitOfWork.Client.Update(viewmodel.Client);
                    _unitOfWork.Save();


                    var Admins = _unitOfWork.Admin.GetAll();
                    foreach (var item in Admins)
                    {
                        var admin = _unitOfWork.ApplicationUser.Get(u => u.AdminID == item.ID);
                        if(admin != null)
                        _emailSender.SendEmailAsync(admin.Email, "new identification- ELawyer", "<p>you have new lawyer want to identification  </p>");
                    }


                    return RedirectToAction("Index");
                    

                }
            }
            return View(viewmodel);
        }

        public IActionResult clientconfirmation()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var Admins = _unitOfWork.Admin.GetAll();

           
            List<ELawyer.Models.Client> ConfirmtList = _unitOfWork.Client.GetAll(c => c.BackCardImage != null && c.FrontCardImage != null && c.UserStatus != SD.UserStatusVerfied).ToList();
          
           
            return View(ConfirmtList);
        }

        public IActionResult Lawyerconfirmation()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var Admins = _unitOfWork.Admin.GetAll();
            List<ELawyer.Models.Lawyer> ConfirmtList = _unitOfWork.Lawyer.GetAll(l => l.BackCardImage != null && l.FrontCardImage != null && l.UserStatus != SD.UserStatusVerfied).ToList();
        
            return View(ConfirmtList);
        }

        public IActionResult Acceptlawyer(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            ELawyer.Models.Lawyer? LawyerFromDb = _unitOfWork.Lawyer.Get(x => x.ID == id);

            
            if (LawyerFromDb == null)
                return NotFound();

            return View(LawyerFromDb);
        }
        [HttpPost]

        public IActionResult Acceptlawyer(int id) 
        {

            
            var lawyer = _unitOfWork.Lawyer.Get(c => c.ID == id);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.LawyerID == lawyer.ID);
            lawyer.UserStatus = SD.UserStatusVerfied;
            _unitOfWork.Lawyer.Update(lawyer);
            _unitOfWork.Save();
            _emailSender.SendEmailAsync(user.Email, "Your Identity has been Identificated - ELawyer","<p>You Can Now Add Your Service </p>");

            return RedirectToAction("Index");
        }

        public IActionResult Rejectlawyer(int? id)
        {
            
            if (id == null || id == 0)
                return NotFound();

            ELawyer.Models.Lawyer? LawyerFromDb = _unitOfWork.Lawyer.Get(x => x.ID == id);

            if (LawyerFromDb == null)
                return NotFound();

            return View(LawyerFromDb);
        }
        [HttpPost]

        public IActionResult Rejectlawyer(int id)
        {

            

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var lawyer = _unitOfWork.Lawyer.Get(c => c.ID == id);
          
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.LawyerID == lawyer.ID);

            if (!string.IsNullOrEmpty(lawyer.FrontCardImage))
            {
                var oldImagePath = Path.Combine(wwwRootPath, lawyer.FrontCardImage.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            if (!string.IsNullOrEmpty(lawyer.BackCardImage))
            {
                var oldImagePath = Path.Combine(wwwRootPath, lawyer.BackCardImage.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            lawyer.FrontCardImage = null;
            lawyer.BackCardImage = null;
            lawyer.UserStatus = SD.UserStatusNotVerfied;
            _unitOfWork.Lawyer.Update(lawyer);
            _unitOfWork.Save();
            _emailSender.SendEmailAsync(user.Email, "Your Identity has been rejected - ELawyer", "<p>please try to upload your Identity card clearly  </p>");

            return RedirectToAction("Index");

            
        }

        public IActionResult Acceptclient(int? id)
        {
            
            if (id == null || id == 0)
                return NotFound();

            ELawyer.Models.Client? clientFromDb = _unitOfWork.Client.Get(x => x.ID == id);

            
            if (clientFromDb == null)
                return NotFound();

            return View(clientFromDb);
        }
        [HttpPost]

        public IActionResult Acceptclient(int id)
        {


            var client = _unitOfWork.Client.Get(c => c.ID == id);
          
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.ClientID == client.ID);
            client.UserStatus = SD.UserStatusVerfied;
            _unitOfWork.Client.Update(client);
            _unitOfWork.Save();

            _emailSender.SendEmailAsync(user.Email, "Your Identity has been Identificated - ELawyer", "<p>Your Identity has been Identificated successfully </p>");

            return RedirectToAction("Index");
        }

        public IActionResult Rejectclient(int? id)
        {
           
            if (id == null || id == 0)
                return NotFound();

            ELawyer.Models.Client? clientFromDb = _unitOfWork.Client.Get(x => x.ID == id);

           
            if (clientFromDb == null)
                return NotFound();

            return View(clientFromDb);
        }
        [HttpPost]

        public IActionResult Rejectclient(int id)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var client = _unitOfWork.Client.Get(c => c.ID == id);

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.ClientID == client.ID);

            if (!string.IsNullOrEmpty(client.FrontCardImage))
            {
                var oldImagePath = Path.Combine(wwwRootPath, client.FrontCardImage.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            if (!string.IsNullOrEmpty(client.BackCardImage))
            {
                var oldImagePath = Path.Combine(wwwRootPath, client.BackCardImage.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            client.FrontCardImage = null;
            client.BackCardImage = null;
            client.UserStatus = SD.UserStatusNotVerfied;
            _unitOfWork.Client.Update(client);
            _unitOfWork.Save();

            _emailSender.SendEmailAsync(user.Email, "Your Identity has been rejected - ELawyer", "<p>please try to upload your Identity card clearly  </p>");


            return RedirectToAction("Index");


        }

    

    }
}

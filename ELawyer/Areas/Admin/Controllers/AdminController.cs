using System.Security.Claims;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models.ViewModels;
using ELawyer.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ELawyer.Areas.Admin.Controllers;

public class AdminController : Controller
{
    private readonly IEmailSender _emailSender;


    private readonly IUnitOfWork _unitOfWork;

    public AdminController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        _emailSender = emailSender;
    }

    public IWebHostEnvironment _webHostEnvironment { get; }

    public IActionResult Index()
    {
        List<Models.Admin> AdminList = _unitOfWork.Admin.GetAll().ToList();


        return View(AdminList);
    }

    public IActionResult Details(int? id)
    {
        var Admin = _unitOfWork.Admin.Get(l => l.Id == id);
        return View(Admin);
    }

    public IActionResult Edit()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Admin = _unitOfWork.Admin.Get(l => l.Id == user.AdminId);
        return View(Admin);
    }

    [HttpPost]
    public IActionResult Edit(Models.Admin newadmin, IFormFile? file)
    {
        var oldAdmin = _unitOfWork.Admin.Get(i => i.Id == newadmin.Id);
        if (oldAdmin == null) return NotFound();

        if (ModelState.IsValid)
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;

            if (file != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var ClientImagePath = Path.Combine(wwwRootPath, @"images\Admin\Profile");


                if (!string.IsNullOrEmpty(oldAdmin.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, oldAdmin.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                }


                using (var fileStream = new FileStream(Path.Combine(ClientImagePath, fileName), FileMode.Create,
                           FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
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


        return View(newadmin);
    }

    public IActionResult Confirmation()
    {
        var viewmodel = new ClientLawyerConfirmation();


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
                var lawyer = _unitOfWork.Lawyer.Get(c => c.Id == user.LawyerId);
                viewmodel.Lawyer = lawyer;

                var wwwRootPath = _webHostEnvironment.WebRootPath;


                if (file1 != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file1.FileName);
                    var lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Cards");


                    if (!string.IsNullOrEmpty(lawyer.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, lawyer.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create,
                               FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
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
                    var fileName = Guid.NewGuid() + Path.GetExtension(file2.FileName);
                    var lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Cards");


                    if (!string.IsNullOrEmpty(lawyer.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, lawyer.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create,
                               FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
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
                    var admin = _unitOfWork.ApplicationUser.Get(u => u.AdminId == item.Id);
                    if (admin != null)
                        _emailSender.SendEmailAsync(admin.Email, "new identification- ELawyer",
                            "<p>you have new lawyer want to identification  </p>");
                }

                return RedirectToAction("Index");
            }

            if (user.Role == SD.ClientRole)
            {
                var client = _unitOfWork.Client.Get(c => c.Id == user.ClientId);
                viewmodel.Client = client;

                var wwwRootPath = _webHostEnvironment.WebRootPath;


                if (file1 != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file1.FileName);
                    var lawyerImagePath = Path.Combine(wwwRootPath, @"images\Lawyer\Cards");


                    if (!string.IsNullOrEmpty(client.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, client.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create,
                               FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
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
                    var fileName = Guid.NewGuid() + Path.GetExtension(file2.FileName);
                    var lawyerImagePath = Path.Combine(wwwRootPath, @"images\Client\Cards");


                    if (!string.IsNullOrEmpty(client.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, client.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(lawyerImagePath, fileName), FileMode.Create,
                               FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
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
                    var admin = _unitOfWork.ApplicationUser.Get(u => u.AdminId == item.Id);
                    if (admin != null)
                        _emailSender.SendEmailAsync(admin.Email, "new identification- ELawyer",
                            "<p>you have new lawyer want to identification  </p>");
                }


                return RedirectToAction("Index");
            }
        }

        return View(viewmodel);
    }

    public IActionResult ClientConifrmation()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var Admins = _unitOfWork.Admin.GetAll();


        List<Models.Client> ConfirmtList = _unitOfWork.Client.GetAll(c =>
            c.BackCardImage != null && c.FrontCardImage != null && c.UserStatus != SD.UserStatusVerfied).ToList();


        return View(ConfirmtList);
    }

    public IActionResult LawyerConfirmation()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var Admins = _unitOfWork.Admin.GetAll();
        List<Models.Lawyer> ConfirmtList = _unitOfWork.Lawyer.GetAll(l =>
            l.BackCardImage != null && l.FrontCardImage != null && l.UserStatus != SD.UserStatusVerfied).ToList();

        return View(ConfirmtList);
    }

    public IActionResult AcceptLawyer(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var LawyerFromDb = _unitOfWork.Lawyer.Get(x => x.Id == id);


        if (LawyerFromDb == null)
            return NotFound();

        return View(LawyerFromDb);
    }

    [HttpPost]
    public IActionResult AcceptLawyer(int id)
    {
        var lawyer = _unitOfWork.Lawyer.Get(c => c.Id == id);
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.LawyerId == lawyer.Id);
        lawyer.UserStatus = SD.UserStatusVerfied;
        _unitOfWork.Lawyer.Update(lawyer);
        _unitOfWork.Save();
        _emailSender.SendEmailAsync(user.Email, "Your Identity has been Identificated - ELawyer",
            "<p>You Can Now Add Your Service </p>");

        return RedirectToAction("Index");
    }

    public IActionResult RejectLawyer(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var LawyerFromDb = _unitOfWork.Lawyer.Get(x => x.Id == id);

        if (LawyerFromDb == null)
            return NotFound();

        return View(LawyerFromDb);
    }

    [HttpPost]
    public IActionResult RejectLawyer(int id)
    {
        var wwwRootPath = _webHostEnvironment.WebRootPath;
        var lawyer = _unitOfWork.Lawyer.Get(c => c.Id == id);

        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.LawyerId == lawyer.Id);

        if (!string.IsNullOrEmpty(lawyer.FrontCardImage))
        {
            var oldImagePath = Path.Combine(wwwRootPath, lawyer.FrontCardImage.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
        }

        if (!string.IsNullOrEmpty(lawyer.BackCardImage))
        {
            var oldImagePath = Path.Combine(wwwRootPath, lawyer.BackCardImage.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
        }

        lawyer.FrontCardImage = null;
        lawyer.BackCardImage = null;
        lawyer.UserStatus = SD.UserStatusNotVerfied;
        _unitOfWork.Lawyer.Update(lawyer);
        _unitOfWork.Save();
        _emailSender.SendEmailAsync(user.Email, "Your Identity has been rejected - ELawyer",
            "<p>please try to upload your Identity card clearly  </p>");

        return RedirectToAction("Index");
    }

    public IActionResult Acceptclient(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var clientFromDb = _unitOfWork.Client.Get(x => x.Id == id);


        if (clientFromDb == null)
            return NotFound();

        return View(clientFromDb);
    }

    [HttpPost]
    public IActionResult AcceptClient(int id)
    {
        var client = _unitOfWork.Client.Get(c => c.Id == id);

        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.ClientId == client.Id);
        client.UserStatus = SD.UserStatusVerfied;
        _unitOfWork.Client.Update(client);
        _unitOfWork.Save();

        _emailSender.SendEmailAsync(user.Email, "Your Identity has been Identificated - ELawyer",
            "<p>Your Identity has been Identificated successfully </p>");

        return RedirectToAction("Index");
    }

    public IActionResult RejectClient(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var clientFromDb = _unitOfWork.Client.Get(x => x.Id == id);


        if (clientFromDb == null)
            return NotFound();

        return View(clientFromDb);
    }

    [HttpPost]
    public IActionResult RejectClient(int id)
    {
        var wwwRootPath = _webHostEnvironment.WebRootPath;
        var client = _unitOfWork.Client.Get(c => c.Id == id);

        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.ClientId == client.Id);

        if (!string.IsNullOrEmpty(client.FrontCardImage))
        {
            var oldImagePath = Path.Combine(wwwRootPath, client.FrontCardImage.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
        }

        if (!string.IsNullOrEmpty(client.BackCardImage))
        {
            var oldImagePath = Path.Combine(wwwRootPath, client.BackCardImage.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
        }

        client.FrontCardImage = null;
        client.BackCardImage = null;
        client.UserStatus = SD.UserStatusNotVerfied;
        _unitOfWork.Client.Update(client);
        _unitOfWork.Save();

        _emailSender.SendEmailAsync(user.Email, "Your Identity has been rejected - ELawyer",
            "<p>please try to upload your Identity card clearly  </p>");


        return RedirectToAction("Index");
    }
}
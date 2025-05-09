using System.Security.Claims;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models.ViewModels;
using ELawyer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELawyer.Areas.Admin.Controllers;

[Authorize(Roles = SD.AdminRole)] // This restricts all actions to admin users only
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

    [Route("admin/dashboard")]
    public IActionResult Index()
    {
        var clients = _unitOfWork.Client.GetAll().ToList();
        var lawyers = _unitOfWork.Lawyer.GetAll().ToList();
        var orders = _unitOfWork.ServiceOrder.GetAll().ToList();
        // Get current month earnings
        var currentMonthEarnings = orders
            .Where(o => o.CreatedAt.Month == DateTime.Now.Month &&
                        o.CreatedAt.Year == DateTime.Now.Year)
            .Sum(o => o.Amount);

        // Get previous month earnings
        var previousMonthEarnings = orders
            .Where(o => o.CreatedAt.Month == DateTime.Now.AddMonths(-1).Month &&
                        o.CreatedAt.Year == DateTime.Now.AddMonths(-1).Year)
            .Sum(o => o.Amount);

        // Calculate percentage change
        decimal percentageChange = 0;
        var increased = true;

        if (previousMonthEarnings > 0)
        {
            percentageChange = (decimal)((currentMonthEarnings - previousMonthEarnings) / previousMonthEarnings) * 100;
            increased = currentMonthEarnings >= previousMonthEarnings;
        }

        // Create dashboard view model with all required data
        var dashboardData = new AdminDashboardVm
        {
            // Get total counts from database
            TotalClients = clients.Count(),
            TotalLawyers = lawyers.Count(),
            TotalOrders = orders.Count(),
            TotalEarnings = orders.Sum(o => o.Amount),
            PendingApprovals = lawyers.Count(l => l.UserStatus == SD.UserStatusPending),
            EarningsPercentageChange = Math.Round(percentageChange, 1),
            EarningsIncreased = increased,

            // Get 5 most recent lawyer registrations
            RecentLawyers = _unitOfWork.Lawyer
                .GetAll(includeproperties: "ApplicationUser")
                .OrderByDescending(l => l.ApplicationUser.CreatedAt)
                .Take(5)
                .Select(l => new LawyerRegistrationVm
                {
                    Id = l.Id,
                    Name = $"{l.ApplicationUser.FirstName} {l.ApplicationUser.LastName}",
                    Email = l.ApplicationUser.Email,
                    Status = l.UserStatus // Verification status
                })
                .ToList(),

            RecentTransactions = orders
                .OrderByDescending(o => o.CreatedAt)
                .Take(10)
                .Select(o => new TransactionVm
                {
                    Date = o.CreatedAt,
                    ClientName = $"{o.Client.ApplicationUser.FirstName} {o.Client.ApplicationUser.LastName}",
                    LawyerName = $"{o.Lawyer.ApplicationUser.FirstName} {o.Lawyer.ApplicationUser.LastName}",
                    ServiceTitle = o.Service.Title,
                    Amount = (decimal)o.Amount,
                    PayedAt = o.Payment.PaidAt
                })
                .ToList(),

            ClientList = new SelectList(clients.Select(c => new
            {
                c.Id,
                Name = $"{c.ApplicationUser.FirstName} {c.ApplicationUser.LastName}"
            }), "Id", "Name"),

            LawyerList = new SelectList(lawyers.Select(l => new
            {
                l.Id,
                Name = $"{l.ApplicationUser.FirstName} {l.ApplicationUser.LastName}"
            }), "Id", "Name")
        };

        return View(dashboardData);
    }

    [Route("admin/clients")]
    public IActionResult Clients()
    {
        return View();
    }

    [Route("admin/lawyers")]
    public IActionResult Lawyers()
    {
        return View();
    }

    [Route("admin/all-users")]
    public IActionResult AllUsers()
    {
        return View();
    }

    [Route("admin/orders")]
    public IActionResult Orders()
    {
        return View();
    }

    [Route("admin/Services")]
    public IActionResult Services()
    {
        return View();
    }

    [Route("admin/details")]
    public IActionResult Details(int? id)
    {
        var Admin = _unitOfWork.Admin.Get(l => l.Id == id);
        return View(Admin);
    }

    [Route("admin/edit")]
    public IActionResult Edit()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        var Admin = _unitOfWork.Admin.Get(l => l.Id == user.Admin.Id);
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
        var viewmodel = new LawyerConfirmation();


        return View(viewmodel);
    }

    [HttpPost]
    public IActionResult Confirmation(LawyerConfirmation viewmodel, IFormFile? file1, IFormFile? file2)
    {
        if (ModelState.IsValid)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);


            if (user.Role == SD.LawyerRole)
            {
                var lawyer = _unitOfWork.Lawyer.Get(c => c.Id == user.Lawyer.Id);
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
                    var admin = _unitOfWork.ApplicationUser.Get(u => u.Admin.Id == item.Id);
                    if (admin != null)
                        _emailSender.SendEmailAsync(admin.Email, "new identification- ELawyer",
                            "<p>you have new lawyer want to identification  </p>");
                }

                return RedirectToAction("Index");
            }
        }

        return View(viewmodel);
    }

    public IActionResult LawyerConfirmation()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var Admins = _unitOfWork.Admin.GetAll();
        var ConfirmtList = _unitOfWork.Lawyer.GetAll(l =>
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
        var user = _unitOfWork.ApplicationUser.Get(u => u.Lawyer.Id == lawyer.Id);
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
        var user = _unitOfWork.ApplicationUser.Get(u => u.Lawyer.Id == lawyer.Id);

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
        var user = _unitOfWork.ApplicationUser.Get(u => u.Client.Id == client.Id);
        client.UserStatus = SD.UserStatusVerfied;
        _unitOfWork.Client.Update(client);
        _unitOfWork.Save();

        _emailSender.SendEmailAsync(user.Email, "Your Identity has been Identificated - ELawyer",
            "<p>Your Identity has been Identificated successfully </p>");

        return RedirectToAction("Index");
    }
}
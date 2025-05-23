﻿using System.Security.Claims;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using ELawyer.Models.ViewModels.Admin.Dashboard;
using ELawyer.Models.ViewModels.Admin.Users;
using ELawyer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ELawyer.Areas.Admin.Controllers;

[Authorize(Roles = SD.AdminRole)] // This restricts all actions to admin users only
public class AdminController : Controller
{
    private readonly IEmailSender _emailSender;

    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(IUnitOfWork unitOfWork, UserManager<IdentityUser> user,
        IWebHostEnvironment webHostEnvironment, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        _emailSender = emailSender;
        _userManager = user;
        _roleManager = roleManager;
    }

    public IWebHostEnvironment _webHostEnvironment { get; }

    [Route("admin/dashboard")]
    public IActionResult Index()
    {
        var clients = _unitOfWork.Client.GetAll(c => c.ApplicationUser != null, "ApplicationUser").ToList();
        var lawyers = _unitOfWork.Lawyer.GetAll(l => true, "ApplicationUser").ToList();
        var orders = _unitOfWork.ServiceOrder
            .GetAll(o => o.Lawyer.ApplicationUser != null && o.Client.ApplicationUser != null,
                "Client.ApplicationUser,Lawyer.ApplicationUser,Service").ToList();

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
            RecentLawyers = lawyers
                .Where(l => l.ApplicationUser != null)
                .OrderByDescending(l => l.ApplicationUser.CreatedAt)
                .Take(5)
                .Select(l => new LawyerRegistrationVm
                {
                    Id = l.Id,
                    Name = $"{l.ApplicationUser.FirstName} {l.ApplicationUser.LastName}",
                    Email = l.ApplicationUser.Email,
                    Status = l.UserStatus
                })
                .ToList(),

            RecentTransactions = orders
                .Where(o => o.Client?.ApplicationUser != null && o.Lawyer?.ApplicationUser != null)
                .OrderByDescending(o => o.CreatedAt)
                .Take(10)
                .Select(o => new TransactionVm
                {
                    Date = o.CreatedAt,
                    ClientName =
                        $"{o.Client.ApplicationUser.FirstName} {o.Client.ApplicationUser.LastName}",
                    LawyerName =
                        $"{o.Lawyer.ApplicationUser.FirstName} {o.Lawyer.ApplicationUser.LastName}",
                    ServiceTitle = o.Service?.Title ?? "N/A",
                    Amount = (decimal)o.Amount,
                    PayedAt = o.Payment?.PaidAt
                })
                .ToList(),

            ClientList = new SelectList(clients.Select(c => new
            {
                c.Id,
                Name = c.ApplicationUser != null
                    ? $"{c.ApplicationUser.FirstName} {c.ApplicationUser.LastName}"
                    : "Unknown Client"
            }), "Id", "Name"),

            LawyerList = new SelectList(lawyers.Select(l => new
            {
                l.Id,
                Name = l.ApplicationUser != null
                    ? $"{l.ApplicationUser.FirstName} {l.ApplicationUser.LastName}"
                    : "Unknown Lawyer"
            }), "Id", "Name")
        };

        return View(dashboardData);
    }


    [Route("admin/users/lawyers")]
    public IActionResult Lawyers()
    {
        var lawyers = _unitOfWork.Lawyer.GetAll(l => true, "ApplicationUser"
        ).ToList();

        return View(lawyers);
    }

    [Produces("text/html")]
    [HttpGet("admin/all-users")]
    public async Task<IActionResult> AllUsers([FromQuery] UserFilter filter)
    {
        try
        {
            var users = await GetUsers(filter);
            return View("AllUsers", users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Route("admin/orders")]
    public IActionResult Orders()
    {
        var orders = _unitOfWork.ServiceOrder.GetAll(s => true,
            "Service,Client.ApplicationUser,Lawyer.ApplicationUser,Payment"
        ).ToList();
        return View(orders);
    }

    [Route("admin/orders/delete/{id}")]
    public IActionResult DeleteOrder(int id)
    {
        var order = _unitOfWork.ServiceOrder.Get(o => o.Id == id);
        if (order == null) return NotFound();
        return View(order);
    }

    [HttpPost("admin/orders/delete/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteOrderConfirmed(int id)
    {
        var order = _unitOfWork.ServiceOrder.Get(o => o.Id == id);
        if (order != null)
        {
            _unitOfWork.ServiceOrder.Remove(order);
            _unitOfWork.Save();
        }

        return RedirectToAction("Index");
    }

    [Route("admin/services")]
    public IActionResult Services()
    {
        var services = _unitOfWork.Service
            .GetAll(s => true, "Lawyer")
            .ToList();
        return View(services);
    }

    [Route("admin/services/edit/{id}")]
    public IActionResult EditService(int id)
    {
        var service = _unitOfWork.Service.Get(s => s.Id == id);
        if (service == null) return NotFound();

        ViewBag.Lawyers = _unitOfWork.Lawyer.GetAll().Select(l => new SelectListItem
        {
            Text = $"{l.ApplicationUser?.FirstName} {l.ApplicationUser?.LastName}",
            Value = l.Id.ToString()
        });
        return View(service);
    }

    [HttpPost("admin/services/edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult EditService(int id, Service service)
    {
        if (id != service.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _unitOfWork.Service.Update(service);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        return View(service);
    }

    [Route("admin/services/delete/{id}")]
    public IActionResult DeleteService(int id)
    {
        var service = _unitOfWork.Service.Get(s => s.Id == id);
        if (service == null) return NotFound();
        return View(service);
    }

    // حذف خدمة (POST)
    [HttpPost("admin/services/delete/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteServiceConfirmed(int id)
    {
        var service = _unitOfWork.Service.Get(s => s.Id == id);
        if (service != null)
        {
            _unitOfWork.Service.Remove(service);
            _unitOfWork.Save();
        }

        return RedirectToAction("Index");
    }

    [Route("admin/details")]
    public IActionResult Details(int? id)
    {
        var Admin = _unitOfWork.Admin.Get(l => l.Id == id);
        return View(Admin);
    }

    [HttpGet("admin/users/edit/{id}")]
    public async Task<IActionResult> EditUser(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new UserEditVm
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? ""
            };

            ViewBag.Roles = new SelectList(allRoles, model.Role);
            return View(model);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpPost("admin/users/edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(string id, UserEditVm model)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.UserName = model.UserName;
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, model.Role);

            return RedirectToAction("AllUsers");
        }
        catch (Exception ex)
        {
            ViewBag.Roles = new SelectList(new List<string> { "Admin", "Client" }, model.Role);
            ModelState.AddModelError("", $"Error: {ex.Message}");
            return View(model);
        }
    }

    [HttpPost("admin/users/delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) throw new Exception("Failed to delete user");

            return RedirectToAction("AllUsers");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
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

    /*
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
    */

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

    /* Helper methods */
    public async Task<List<UserItemVm>> GetUsers(UserFilter filter)
    {
        var usersQuery = _userManager.Users.AsQueryable();

        // Filter by role
        if (!string.IsNullOrEmpty(filter.Role))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(filter.Role);
            var userIds = usersInRole.Select(u => u.Id).ToHashSet();
            usersQuery = usersQuery.Where(u => userIds.Contains(u.Id));
        }

        // Filter by search term
        if (!string.IsNullOrEmpty(filter.SearchTerm))
            usersQuery = usersQuery.Where(u =>
                u.UserName.Contains(filter.SearchTerm) ||
                u.Email.Contains(filter.SearchTerm));

        // Execute query first to avoid multiple active readers
        var usersList = await usersQuery.ToListAsync();

        // Now get roles for each user
        var result = new List<UserItemVm>();

        foreach (var user in usersList)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserItemVm
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = string.Join(", ", roles)
            });
        }

        return result;
    }
}
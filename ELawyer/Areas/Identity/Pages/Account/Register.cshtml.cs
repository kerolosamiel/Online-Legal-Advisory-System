﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using ELawyer.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;

namespace ELawyer.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly IUserEmailStore<IdentityUser> _emailStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;

    public RegisterModel(
        RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        SignInManager<IdentityUser> signInManager,
        ILogger<RegisterModel> logger,
        IWebHostEnvironment WebHostEnvironment,
        IUnitOfWork unitOfWork,
        IEmailSender emailSender
    )
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        this.WebHostEnvironment = WebHostEnvironment;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }

    public IWebHostEnvironment WebHostEnvironment { get; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme> ExternalLogins { get; set; }


    public async Task OnGetAsync(string returnUrl = null)
    {
        Input = new InputModel
        {
            RoleList = (User.IsInRole(SD.AdminRole)
                    ? _roleManager.Roles.Select(x => x.Name)
                    : _roleManager.Roles.Select(x => x.Name).Where(r => r == SD.ClientRole || r == SD.LawyerRole)
                ).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
        };


        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }


    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (ModelState.IsValid)
        {
            var user = await CreateUserAsync();

            var wwwRootPath = WebHostEnvironment.WebRootPath;


            if (Input.Role == SD.AdminRole)
            {
                user.Admin = new Models.Admin
                {
                    ApplicationUser = user
                };

                user.Role = SD.AdminRole;
                user.Admin.Address = Input.Address;
            }

            if (Input.Role == SD.ClientRole)
            {
                user.Client = new Models.Client
                {
                    ApplicationUser = user
                };

                user.Role = SD.ClientRole;
                user.Client.Address = Input.Address;
                user.Client.UserStatus = SD.UserStatusActive;
            }

            if (Input.Role == SD.LawyerRole)
            {
                user.Lawyer = new Models.Lawyer
                {
                    ApplicationUser = user
                };

                user.Role = SD.LawyerRole;
                user.Lawyer.UserStatus = SD.UserStatusNotVerfied;
                user.Lawyer.Address = Input.Address;
            }


            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                if (!string.IsNullOrEmpty(Input.Role)) await _userManager.AddToRoleAsync(user, Input.Role);


                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/account/ConfirmEmail",
                    null,
                    new { area = "Identity", userId, code, returnUrl },
                    Request.Scheme);

                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                // comment after edit

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });

                if (User.IsInRole(SD.AdminRole))
                    TempData["success"] = "New User Created Successfully";
                else
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("/account/Login");


                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    private async Task<ApplicationUser> CreateUserAsync()
    {
        try
        {
            var user = Activator.CreateInstance<ApplicationUser>();

            // Generate unique username
            Input.UserName = await GenerateUniqueUsername(Input.FirstName, Input.LastName);
            await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.CreatedAt = DateTime.Now;

            return user;
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                                                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Views/account/Register.cshtml");
        }
    }

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
            throw new NotSupportedException("The default UI requires a user store with email support.");
        return (IUserEmailStore<IdentityUser>)_userStore;
    }

    private async Task<string> GenerateUniqueUsername(string firstName, string lastName)
    {
        var baseUsername = $"{firstName.ToLower()}_{lastName.ToLower()}".Replace(" ", "_");
        var username = baseUsername;
        var counter = 1;

        while (await _userManager.FindByNameAsync(username) != null)
        {
            username = $"{baseUsername}{counter}";
            counter++;
        }

        return username;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        [Display(Name = "Username")] public string UserName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public string Address { get; set; }

        public DateTime? CreatedAt { get; set; }


        //Note:
        //Add Drop Down List of Roles

        public string Role { get; set; }

        [ValidateNever] public IEnumerable<SelectListItem> RoleList { get; set; }

        //[Required]
        public List<int> Specialization { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> SpecializationList { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.RegularExpressions;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ELawyer.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;

    public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }


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
    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string ErrorMessage { get; set; }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage)) ModelState.AddModelError(string.Empty, ErrorMessage);

        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            // Find user by email or username
            var user = await _userManager.FindByEmailAsync(Input.LoginIdentifier)
                       ?? await _userManager.FindByNameAsync(Input.LoginIdentifier);

            // Add manual format validation
            if (!IsValidLoginIdentifier(Input.LoginIdentifier))
            {
                ModelState.AddModelError("Input.LoginIdentifier", "Invalid username or email format");
                return Page();
            }


            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            if (user != null)
            {
                // Use the actual username for sign-in (required by Identity)
                var result = await _signInManager.PasswordSignInAsync(
                    user?.UserName ?? "", // Must use UserName here
                    Input.Password,
                    Input.RememberMe,
                    false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");


                    // Your existing last login tracking code
                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var userFromDb = _unitOfWork.ApplicationUser.Get(
                        u => u.Id == user.Id, $"{nameof(Admin)},{nameof(Lawyer)},{nameof(Client)}"
                    );

                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains(SD.AdminRole))
                    {
                        userFromDb.Admin.ApplicationUser.LastLogin = DateTime.Now;
                        _unitOfWork.Admin.Update(userFromDb.Admin);
                        _unitOfWork.Save();

                        return RedirectToAction("Index", "Admin");
                    }

                    if (roles.Contains(SD.ClientRole))
                    {
                        userFromDb.Client.ApplicationUser.LastLogin = DateTime.Now;
                        _unitOfWork.Client.Update(userFromDb.Client);
                        _unitOfWork.Save();

                        return LocalRedirect(returnUrl);
                    }

                    if (roles.Contains(SD.LawyerRole))
                    {
                        userFromDb.Lawyer.ApplicationUser.LastLogin = DateTime.Now;
                        _unitOfWork.Lawyer.Update(userFromDb.Lawyer);
                        _unitOfWork.Save();

                        return LocalRedirect(returnUrl);
                    }
                }

                if (result.RequiresTwoFactor)
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
            }


            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return Page();
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    private bool IsValidLoginIdentifier(string input)
    {
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        var usernameRegex = new Regex(@"^[a-zA-Z0-9_]{3,20}$");
        return emailRegex.IsMatch(input) || usernameRegex.IsMatch(input);
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        [Required(ErrorMessage = "Please enter your username or email")]
        [Display(Name = "Username or Email")]
        public string LoginIdentifier { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
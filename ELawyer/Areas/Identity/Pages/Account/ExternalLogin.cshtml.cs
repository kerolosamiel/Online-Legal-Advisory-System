﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using ELawyer.Models;
using ELawyer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;

namespace ELawyer.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ExternalLoginModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly IUserEmailStore<IdentityUser> _emailStore;
    private readonly ILogger<ExternalLoginModel> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;

    public ExternalLoginModel(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        ILogger<ExternalLoginModel> logger,
        IEmailSender emailSender,
        RoleManager<IdentityRole> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _logger = logger;
        _emailSender = emailSender;
        _roleManager = roleManager;
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
    public string ProviderDisplayName { get; set; }

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

    public IActionResult OnGet()
    {
        return RedirectToPage("./Login");
    }

    public IActionResult OnPost(string provider, string returnUrl = null)
    {
        // Request a redirect to the external login provider.
        var redirectUrl = Url.Page("./ExternalLogin", "Callback", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        if (remoteError != null)
        {
            ErrorMessage = $"Error from external provider: {remoteError}";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Error loading external login information.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
        if (result.Succeeded)
        {
            _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name,
                info.LoginProvider);
            return LocalRedirect(returnUrl);
        }

        if (result.IsLockedOut) return RedirectToPage("./Lockout");

        // If the user does not have an account, then ask the user to create an account.
        ReturnUrl = returnUrl;
        ProviderDisplayName = info.ProviderDisplayName;
        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        {
            Input = new InputModel
            {
                Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                FirstName = info.Principal.FindFirstValue(ClaimTypes.Name)
            };

            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.LawyerRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.ClientRole)).GetAwaiter().GetResult();
            }

            //Note:
            //Add Drop Down List of Roles
            Input = new InputModel
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
        }

        ;
        return Page();
    }


    public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Error loading external login information during confirmation.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        if (ModelState.IsValid)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);


            if (Input.Role == SD.AdminRole)
            {
                if (user.Admin == null) user.Admin = new Models.Admin();


                user.Role = SD.AdminRole;
                user.Admin.ApplicationUser.FirstName = Input.FirstName;
                user.Admin.ApplicationUser.LastName = Input.LastName;
                user.Admin.Address = Input.Address;
                user.Admin.ApplicationUser.CreatedAt = DateTime.Now;
            }

            if (Input.Role == SD.ClientRole)
            {
                if (user.Client == null) user.Client = new Models.Client();


                user.Role = SD.ClientRole;
                user.Client.ApplicationUser.FirstName = Input.FirstName;
                user.Client.ApplicationUser.LastName = Input.LastName;
                user.Client.Address = Input.Address;
                user.Client.ApplicationUser.CreatedAt = DateTime.Now;
                //user.Client.AccountStatus = SD.AccountStatusActive;
            }

            if (Input.Role == SD.LawyerRole)
            {
                if (user.Lawyer == null) user.Lawyer = new Models.Lawyer();


                user.Role = SD.LawyerRole;
                user.Lawyer.ApplicationUser.FirstName = Input.FirstName;
                user.Lawyer.ApplicationUser.LastName = Input.LastName;


                //user.Lawyer.AccountStatus = SD.AccountStatusActive;
                user.Lawyer.Address = Input.Address;
                user.Lawyer.ApplicationUser.CreatedAt = DateTime.Now;
            }


            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/account/ConfirmEmail",
                        null,
                        new { area = "Identity", userId, code },
                        Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    // If account confirmation is required, we need to show the link if we don't have a real email sender
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        return RedirectToPage("./RegisterConfirmation", new { Input.Email });

                    await _signInManager.SignInAsync(user, false, info.LoginProvider);
                    return LocalRedirect(returnUrl);
                }
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        ProviderDisplayName = info.ProviderDisplayName;
        ReturnUrl = returnUrl;
        return Page();
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                                                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the external login page in /Areas/Identity/Views/account/ExternalLogin.cshtml");
        }
    }

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
            throw new NotSupportedException("The default UI requires a user store with email support.");
        return (IUserEmailStore<IdentityUser>)_userStore;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required] public string FirstName { get; set; }

        //[Required]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string Role { get; set; }

        [ValidateNever] public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
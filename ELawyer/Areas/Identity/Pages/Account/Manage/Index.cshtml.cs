// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using ELawyer.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ELawyer.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;

    public IndexModel(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    private async Task LoadAsync(IdentityUser user)
    {
        var userName = await _userManager.GetUserNameAsync(user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        Username = userName;
        var userEntity = _unitOfWork.ApplicationUser.Get(u => u.Id == user.Id, "Lawyer,Client");

        if (userEntity == null)
            throw new Exception("User not found.");

        if (userEntity.Role == SD.LawyerRole)
        {
            if (userEntity.Lawyer == null)
                throw new Exception("User is not associated with a lawyer profile.");

            var lawyerId = userEntity.Lawyer.Id;
            var lawyer = _unitOfWork.Lawyer.Get(l => l.Id == lawyerId);
            var lawyerService = _unitOfWork.Service.Get(s => s.Id == lawyer.ServiceId, "ApplicationUser");

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Lawyer = lawyer,
                LawyerService = lawyerService
            };
        }
        else if (userEntity.Role == SD.ClientRole)
        {
            var client = _unitOfWork.Client.Get(c => c.Id == userEntity.Client.Id);
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Client = client
            };
        }
        else
        {
            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        // specializations
        //Input = new()
        //{


        //    SpecializationList = _unitOfWork.specializationnew.GetAll().Select(i => new SelectListItem
        //    {
        //        Text = i.Name,
        //        Value = i.ID.ToString()
        //    }).ToList()

        //};


        //Input = new()
        //{

        //};


        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);

            return Page();
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public Models.Client Client { get; set; }
        public Models.Lawyer Lawyer { get; set; }
        public Service LawyerService { get; set; }
    }
}
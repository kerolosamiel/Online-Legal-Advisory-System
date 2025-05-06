using System.Diagnostics;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.DbInitilizer;
using ELawyer.Models;
using ELawyer.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Dbinitilizer;

public class DbInitializer : IDbInitilizer
{
    private readonly ApplicationDbContext _db;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public DbInitializer(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext db)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _db = db;
    }

    //Note:
    //This method resbonsible for creating admin and roles in our website

    public void Initilalize()
    {
        //migrations if they are not applied
        try
        {
            if (_db.Database.GetPendingMigrations().Count() > 0) _db.Database.Migrate();
        }
        catch (Exception ex)
        {
            // Log the exception
            Trace.WriteLine($"Migration failed: {ex.Message}");
            return;
        }


        //create roles if they are not created
        if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(SD.ClientRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.LawyerRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();


            // Create admin user
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@admin.com",
                FirstName = "admin",
                LastName = "admin",
                CreatedAt = DateTime.Now,
                EmailConfirmed = true
            };

            var result = _userManager.CreateAsync(adminUser, "Admin123#").GetAwaiter().GetResult();

            if (result.Succeeded)
            {
                // Create admin record linked to this user
                var admin = new Admin
                {
                    ApplicationUser = adminUser
                };

                _db.Admins.Add(admin);
                _db.SaveChanges();

                // Update the user with AdminId
                adminUser.AdminId = admin.Id;
                _userManager.UpdateAsync(adminUser).GetAwaiter().GetResult();

                // Assign admin role
                var roleResult = _userManager.AddToRoleAsync(adminUser, SD.AdminRole).GetAwaiter().GetResult();

                if (!roleResult.Succeeded)
                {
                    Trace.WriteLine("Failed to assign admin role:");
                    foreach (var error in roleResult.Errors) Console.WriteLine(error.Description);
                }
            }
            else
            {
                // Log errors
                Trace.WriteLine("Failed to create admin user:");
                foreach (var error in result.Errors) Trace.WriteLine(error.Description);
            }
        }
    }
}
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.DbInitilizer;
using ELawyer.Models;
using ELawyer.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Dbinitilizer
{
    public class DbInitializer : IDbInitilizer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

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
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }



            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.ClientRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.LawyerRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();


                Admin admin = new Admin()
                {
                    FirstName = "Mohamed",
                    LastName = "Saad",
                    CreatedAt = DateTime.Now

                }
                ;

                _db.Admins.Add(admin);
                _db.SaveChanges();

                //if roles are not created, then we will create admin user as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "mohameds3add@gmail.com",
                    Email = "mohameds3add@gmail.com",
                    EmailConfirmed = true,
                    AdminID =1
                    

                }, "Admin123#").GetAwaiter().GetResult();

                
                


                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "mohameds3add@gmail.com");
                _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();

            }

            return;
        }
    }

}


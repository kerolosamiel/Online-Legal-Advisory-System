using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.DataAccess.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Admin obj)
        {
            Admin admin = _context.Admins.FirstOrDefault(u => u.ID == obj.ID);
            if (admin != null)
            {


                admin.FirstName = obj.FirstName;
                admin.LastName = obj.LastName;
                admin.Address = obj.Address;
                admin.Address = obj.Address;
                admin.LastLogin = obj.LastLogin;
            }
        }
    }
}

using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using Microsoft.EntityFrameworkCore;

namespace ELawyer.DataAccess.Repository;

public class AdminRepository : Repository<Admin>, IAdminRepository
{
    private readonly ApplicationDbContext _context;

    public AdminRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Admin obj)
    {
        var admin = _context.Admins.Include(admin => admin.ApplicationUser).FirstOrDefault(u => u.Id == obj.Id);
        if (admin != null)
        {
            admin.Address = obj.Address;
            admin.Address = obj.Address;
            admin.ApplicationUser.LastLogin = obj.ApplicationUser.LastLogin;
        }
    }
}
using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using Microsoft.EntityFrameworkCore;

namespace ELawyer.DataAccess.Repository;

public class LawyerRepository : Repository<Lawyer>, ILawyerRepository
{
    private readonly ApplicationDbContext _context;

    public LawyerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Lawyer obj)
    {
        var lawyer = _context.Lawyers.Include(lawyer => lawyer.ApplicationUser).FirstOrDefault(u => u.Id == obj.Id);
        if (lawyer != null)
        {
            lawyer.ApplicationUser.FirstName = obj.ApplicationUser.FirstName;
            lawyer.ApplicationUser.LastName = obj.ApplicationUser.LastName;
            lawyer.Address = obj.Address;
            lawyer.Address = obj.Address;
            lawyer.LinceseNumber = obj.LinceseNumber;
            lawyer.FrontCardImage = obj.FrontCardImage;
            lawyer.BackCardImage = obj.BackCardImage;
            lawyer.ImageUrl = obj.ImageUrl;
            lawyer.UserStatus = obj.UserStatus;
            lawyer.LinkedIn = obj.LinkedIn;
            lawyer.About = obj.About;
            lawyer.ConsultationFee = obj.ConsultationFee;
            lawyer.ExperienceYears = obj.ExperienceYears;
            lawyer.ApplicationUser.LastLogin = obj.ApplicationUser.LastLogin;
            lawyer.ServiceId = obj.ServiceId;
            lawyer.NoOfClients = obj.NoOfClients;
            if (obj.AverageRateing == null)
                lawyer.AverageRateing = 0;
            else
                lawyer.AverageRateing = obj.AverageRateing;
            lawyer.LawyerRatingId = obj.LawyerRatingId;
        }
    }
}
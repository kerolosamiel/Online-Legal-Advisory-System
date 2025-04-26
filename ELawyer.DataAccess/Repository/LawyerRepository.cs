using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.DataAccess.Repository
{
    public class LawyerRepository : Repository<Lawyer>, ILawyerRepository
    {
        private readonly ApplicationDbContext _context;

        public LawyerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Lawyer obj)
        {
            Lawyer lawyer = _context.Lawyers.FirstOrDefault(u => u.ID == obj.ID);
            if (lawyer != null)
            {


                lawyer.FirstName = obj.FirstName;
                lawyer.LastName = obj.LastName;
                lawyer.Address = obj.Address;
                lawyer.Address = obj.Address;
                lawyer.LinceseNumber = obj.LinceseNumber;
                lawyer.FrontCardImage = obj.FrontCardImage;
                lawyer.BackCardImage = obj.BackCardImage;
                lawyer.ImageUrl = obj.ImageUrl;
                lawyer.UserStatus = obj.UserStatus;
                lawyer.Linkedin = obj.Linkedin;
                lawyer.About = obj.About;
                lawyer.ConsultationFee = obj.ConsultationFee;
                lawyer.ExperienceYears = obj.ExperienceYears;
             
                lawyer.LastLogin = obj.LastLogin;
                lawyer.ServiceID = obj.ServiceID;
                lawyer.NoOfClients = obj.NoOfClients;   
                if (obj.AverageRateing == null)
                lawyer.AverageRateing = 0;
                else
                    lawyer.AverageRateing = obj.AverageRateing;
                lawyer.LawyerRatingID = obj.LawyerRatingID;
              
               
            }
            
        }

    }
}

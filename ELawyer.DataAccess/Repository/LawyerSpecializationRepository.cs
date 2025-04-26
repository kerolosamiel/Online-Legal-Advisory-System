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
    public class LawyerSpecializationRepository : Repository<LawyerSpecialization>, ILawyerSpecializationRepository
    {


        private readonly ApplicationDbContext _context;

        public LawyerSpecializationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(LawyerSpecialization obj)
        {
            LawyerSpecialization LawyerSpecialization = _context.lawyerSpecializations.FirstOrDefault(u => u.LawyerId == obj.LawyerId);
            if (LawyerSpecialization != null)
            {
                obj.LawyerId = LawyerSpecialization.LawyerId;
                obj.SpecializationId = LawyerSpecialization.SpecializationId;


            }
        }
    }
}

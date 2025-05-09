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
    public class ConsultationRepository : Repository<Consultation>, IConsultationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsultationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Consultation obj)
        {
            Consultation Consultation = _context.Consultations.FirstOrDefault(u => u.Id == obj.Id);
            if (Consultation != null)
            {



            }
        }
        

       
    }
}

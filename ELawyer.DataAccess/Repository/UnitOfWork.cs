using ELawyer.DataAccess;
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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IAdminRepository Admin { get; private set; }
        public IClientRepository Client { get; private set; }
        public ILawyerRepository Lawyer { get; private set; }
        public ISpecializationRepository specializationnew { get; private set; }


        public IServiceRepository service { get; private set; }
        public IapplicationUserRepository ApplicationUser { get; private set; }
        public IRatingRepository Rating { get; private set; }
        public IPaymentRepository Payment { get; private set; }
        public IConsultationRepository Consultation { get; private set; }
        public IResponseRepository Response { get; private set; }
        public ILawyerSpecializationRepository lawyerSpecialization { get; private set; }
         public IInvoiceRepository invoice { get; private set; }

          public IServiceOrderRepository serviceOrder { get; private set; }

       public UnitOfWork(ApplicationDbContext context) 
        {
            _context = context;
            Admin = new AdminRepository(context);
            Client = new ClientRepository(context);
            Lawyer = new LawyerRepository(context);
            specializationnew = new SpecializationRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
            service = new ServiceRepository(context);
            Rating = new RatingRepository(context);
            lawyerSpecialization = new LawyerSpecializationRepository(context);
            Payment = new PaymentRepository(context);
            Consultation = new ConsultationRepository(context);
            Response = new ResponseRepository(context);
              invoice = new InvoiceRepository(context);
              serviceOrder = new ServiceOrderRepository(context);
 
        }

       

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

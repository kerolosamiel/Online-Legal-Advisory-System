using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.DataAccess.Repository
{
    public class ServiceRepository:  Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

       
        public void Update(Service obj)
        {
            Service service = _context.Services.FirstOrDefault(u => u.ID == obj.ID);
            if (service != null)
            {
                service.Title = obj.Title;
                service.Description = obj.Description;
                
                service.Status = obj.Status;
                service.ServiceType = obj.ServiceType;
                service.Duration = obj.Duration;
               

              

            }
        }

       
    }
}

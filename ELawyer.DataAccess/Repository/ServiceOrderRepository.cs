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
    public class ServiceOrderRepository : Repository<ServiceOrders>, IServiceOrderRepository
    {


        private readonly ApplicationDbContext _context;

        public ServiceOrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ServiceOrders obj)
        {
            ServiceOrders serviceorder = _context.ServiceOrders.FirstOrDefault(u => u.ID == obj.ID);
            if (serviceorder != null)
            {

                serviceorder.ID = obj.ID;
            }
        }
    }
}

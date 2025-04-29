using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;

namespace ELawyer.DataAccess.Repository
{
    public class ServiceOrderRepository(ApplicationDbContext context)
        : Repository<ServiceOrder>(context), IServiceOrderRepository
    {
        private readonly ApplicationDbContext _context = context;

        public void Update(ServiceOrder obj)
        {
            var serviceOrder = _context.ServiceOrders.FirstOrDefault(s => s.Id == obj.Id);
            if (serviceOrder != null)
            {
                serviceOrder.Id = obj.Id;
            }
        }
    }
}

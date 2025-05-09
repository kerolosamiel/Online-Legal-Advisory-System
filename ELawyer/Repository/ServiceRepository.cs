using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;

namespace ELawyer.DataAccess.Repository;

public class ServiceRepository : Repository<Service>, IServiceRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public void Update(Service obj)
    {
        var service = _context.Services.FirstOrDefault(u => u.Id == obj.Id);
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
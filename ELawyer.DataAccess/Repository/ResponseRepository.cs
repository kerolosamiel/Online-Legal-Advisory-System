using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;

namespace ELawyer.DataAccess.Repository;

public class ResponseRepository : Repository<Response>, IResponseRepository
{
    private readonly ApplicationDbContext _context;

    public ResponseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Response obj)
    {
        var response = _context.Responses.FirstOrDefault(u => u.Id == obj.Id);
        if (response != null)
        {
        }
    }
}
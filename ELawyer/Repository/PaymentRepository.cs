using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;

namespace ELawyer.DataAccess.Repository;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Payment obj)
    {
        var pays = _context.Payments.FirstOrDefault(u => u.Id == obj.Id);
        if (pays != null)
        {
            pays.ClientId = obj.ClientId;
            pays.LawyerId = obj.LawyerId;
            pays.Amount = obj.Amount;
            if (pays.PaidAt == null)
                pays.PaidAt = DateTime.Now;
            pays.Recievedat = DateTime.Now;
        }
    }
}
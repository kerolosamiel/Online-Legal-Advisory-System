using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;

namespace ELawyer.DataAccess.Repository;

public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
{
    private readonly ApplicationDbContext _context;

    public InvoiceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Invoice obj)
    {
        var invoice = _context.Invoices.FirstOrDefault(u => u.Id == obj.Id);
        if (invoice != null) invoice.PaymentDate = DateTime.Now;
    }
}
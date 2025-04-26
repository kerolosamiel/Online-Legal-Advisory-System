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
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Invoice obj)
        {
            Invoice invoice = _context.Invoices.FirstOrDefault(u => u.ID == obj.ID);
            if (invoice != null)
            {

                invoice.CreatedAt = DateTime.Now;

            }
        }
    }
}

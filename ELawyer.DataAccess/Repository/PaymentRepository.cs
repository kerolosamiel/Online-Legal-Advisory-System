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
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Payment obj)
        {
            Payment pays = _context.Payments.FirstOrDefault(u => u.Id == obj.Id);
            if (pays != null)
            {

                pays.ClientID = obj.ClientID;
                pays.lawyerID = obj.lawyerID;
                pays.Amount = obj.Amount;
                if(pays.PaidAt == null)
                pays.PaidAt = DateTime.Now;
                pays.Recievedat = DateTime.Now;



            }
        }
    }
}

using ELawyer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.DataAccess.Repository.IRepository
{
   public interface IInvoiceRepository : IRepository<Invoice>
    {
        void Update(Invoice obj);
    }
}

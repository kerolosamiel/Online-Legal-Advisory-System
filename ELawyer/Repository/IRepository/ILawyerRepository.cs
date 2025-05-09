using ELawyer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.DataAccess.Repository.IRepository
{
    public interface ILawyerRepository : IRepository<Lawyer>
    {
        void Update(Lawyer obj);
    }
}

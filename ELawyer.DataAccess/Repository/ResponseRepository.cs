using Azure;
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
    public class ResponseRepository : Repository<Models.Response>, IResponseRepository
    {
        private readonly ApplicationDbContext _context;

        public ResponseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Models.Response obj)
        {
            Models.Response response = _context.Responses.FirstOrDefault(u => u.ID == obj.ID);
            if (response != null)
            {




            }

        }

    }    
}

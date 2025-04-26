using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.DataAccess.Repository
{
    public class SpecializationRepository : Repository<Specializationnew>, ISpecializationRepository
    {
        private readonly ApplicationDbContext _context;

    public SpecializationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    

    }
}

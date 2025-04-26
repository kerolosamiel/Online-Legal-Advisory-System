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
   public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Rating obj)
        {
            Rating rating = _context.Ratings.FirstOrDefault(u => u.ID == obj.ID);
            if (rating != null)
            {
                rating.Comment = obj.Comment;
                rating.Rate = obj.Rate;
                rating.ClientID = obj.ClientID;
                rating.lawyerID = obj.lawyerID;
                rating.CreatedAt = DateTime.Now;
            }
        }

    }
}

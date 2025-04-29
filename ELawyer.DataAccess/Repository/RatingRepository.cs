using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;

namespace ELawyer.DataAccess.Repository;

public class RatingRepository : Repository<Rating>, IRatingRepository
{
    private readonly ApplicationDbContext _context;

    public RatingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Rating obj)
    {
        var rating = _context.Ratings.FirstOrDefault(u => u.ID == obj.ID);
        if (rating != null)
        {
            rating.Comment = obj.Comment;
            rating.Rate = obj.Rate;
            rating.ClientId = obj.ClientId;
            rating.LawyerId = obj.LawyerId;
            rating.CreatedAt = DateTime.Now;
        }
    }
}
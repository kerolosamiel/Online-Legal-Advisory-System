using BookStore.DataAccess.Repository;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;

namespace ELawyer.DataAccess.Repository;

public class LawyerSpecializationRepository : Repository<LawyerSpecialization>, ILawyerSpecializationRepository
{
    private readonly ApplicationDbContext _context;

    public LawyerSpecializationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(LawyerSpecialization obj)
    {
        var LawyerSpecialization = _context.LawyerSpecializations.FirstOrDefault(u => u.LawyerId == obj.LawyerId);
        if (LawyerSpecialization != null)
        {
            obj.LawyerId = LawyerSpecialization.LawyerId;
            obj.SpecializationId = LawyerSpecialization.SpecializationId;
        }
    }
}
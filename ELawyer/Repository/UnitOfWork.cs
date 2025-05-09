using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;

namespace ELawyer.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Admin = new AdminRepository(context);
        Client = new ClientRepository(context);
        Lawyer = new LawyerRepository(context);
        Specializationnew = new SpecializationRepository(context);
        ApplicationUser = new ApplicationUserRepository(context);
        Service = new ServiceRepository(context);
        Rating = new RatingRepository(context);
        LawyerSpecialization = new LawyerSpecializationRepository(context);
        Payment = new PaymentRepository(context);
        Consultation = new ConsultationRepository(context);
        Response = new ResponseRepository(context);
        Invoice = new InvoiceRepository(context);
        ServiceOrder = new ServiceOrderRepository(context);
    }

    public IAdminRepository Admin { get; }
    public IClientRepository Client { get; }
    public ILawyerRepository Lawyer { get; }
    public ISpecializationRepository Specializationnew { get; }
    public IServiceRepository Service { get; }
    public IapplicationUserRepository ApplicationUser { get; }
    public IRatingRepository Rating { get; }
    public IPaymentRepository Payment { get; }
    public IConsultationRepository Consultation { get; }
    public IResponseRepository Response { get; }
    public ILawyerSpecializationRepository LawyerSpecialization { get; }
    public IInvoiceRepository Invoice { get; }

    public IServiceOrderRepository ServiceOrder { get; }


    public void Save()
    {
        _context.SaveChanges();
    }
}
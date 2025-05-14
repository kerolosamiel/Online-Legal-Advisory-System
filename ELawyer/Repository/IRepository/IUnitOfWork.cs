namespace ELawyer.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    IAdminRepository Admin { get; }
    IClientRepository Client { get; }
    ILawyerRepository Lawyer { get; }
    ISpecializationRepository Specializationnew { get; }
    IapplicationUserRepository ApplicationUser { get; }
    IServiceRepository Service { get; }
    IRatingRepository Rating { get; }

    IPaymentRepository Payment { get; }

    IConsultationRepository Consultation { get; }

    IResponseRepository Response { get; }
    ILawyerSpecializationRepository LawyerSpecialization { get; }
    IInvoiceRepository Invoice { get; }

    IServiceOrderRepository ServiceOrder { get; }
    void Save();
    Task SaveAsync();
}
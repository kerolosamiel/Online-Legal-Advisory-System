using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IAdminRepository Admin { get; }
        IClientRepository Client { get; }
        ILawyerRepository Lawyer { get; }
        ISpecializationRepository specializationnew { get; }
        IapplicationUserRepository ApplicationUser { get; }
        IServiceRepository service { get; }
        IRatingRepository Rating { get; }

        IPaymentRepository Payment { get; }

        IConsultationRepository Consultation { get; }

        IResponseRepository Response { get; }
        ILawyerSpecializationRepository lawyerSpecialization { get; }
        void Save();
    }
}

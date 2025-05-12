using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using ELawyer.Models.ViewModels.Admin.Client;
using ELawyer.Utility;

namespace ELawyer.Services.Admin;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _unitOfWork;

    public ClientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<ClientListVm> GetClientsWithPagination(ClientFilter filter, int page, int pageSize)
    {
        var query = _unitOfWork.Client.GetAll(c => true, "ApplicationUser");

        if (!string.IsNullOrEmpty(filter.SearchTerm))
            query = query.Where(c =>
                c.ApplicationUser.FirstName.Contains(filter.SearchTerm) ||
                c.ApplicationUser.LastName.Contains(filter.SearchTerm) ||
                (c.ApplicationUser.FirstName + " " + c.ApplicationUser.LastName).Contains(filter.SearchTerm)
            );

        var totalCount = query.Count();

        var clients = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ClientItemVm
            {
                Id = c.Id,
                FullName = $"{c.ApplicationUser.FirstName} {c.ApplicationUser.LastName}",
                Email = c.ApplicationUser.Email,
                RegistrationDate = c.ApplicationUser.CreatedAt,
                Status = c.UserStatus
            })
            .ToList();

        return Task.FromResult(new ClientListVm
        {
            Clients = new PaginatedList<ClientItemVm>(clients, totalCount, page, pageSize),
            Filter = filter
        });
    }

    public Task<ClientDetailsVm> GetClientDetails(int id)
    {
        var client = _unitOfWork.Client
            .GetAll(c => c.Id == id, "ApplicationUser")
            .FirstOrDefault();

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        return Task.FromResult(new ClientDetailsVm
        {
            Id = client.Id,
            FullName = $"{client.ApplicationUser.FirstName} {client.ApplicationUser.LastName}",
            Email = client.ApplicationUser.Email,
            Phone = client.ApplicationUser.PhoneNumber,
            RegistrationDate = client.ApplicationUser.CreatedAt,
            Status = client.UserStatus,
            ActiveCases = client.UserStatus == "Active" ? 1 : 0
        });
    }

    public Task<int> CreateClient(ClientCreateVm model)
    {
        var applicationUser = new ApplicationUser
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.Phone,
            CreatedAt = DateTime.UtcNow
        };

        _unitOfWork.ApplicationUser.Add(applicationUser);
        _unitOfWork.Save();

        var client = new Client
        {
            UserId = applicationUser.Id,
            UserStatus = SD.UserStatusActive // حالة افتراضية
        };

        _unitOfWork.Client.Add(client);
        _unitOfWork.Save();

        return Task.FromResult(client.Id);
    }

    public Task<bool> UpdateClient(ClientEditVm model)
    {
        var client = _unitOfWork.Client
            .GetAll(c => c.Id == model.Id, "ApplicationUser")
            .FirstOrDefault();

        if (client == null)
            return Task.FromResult(false);

        client.ApplicationUser.FirstName = model.FirstName;
        client.ApplicationUser.LastName = model.LastName;
        client.ApplicationUser.Email = model.Email;
        client.ApplicationUser.PhoneNumber = model.Phone;

        _unitOfWork.ApplicationUser.Update(client.ApplicationUser);
        _unitOfWork.Client.Update(client);
        _unitOfWork.Save();

        return Task.FromResult(true);
    }

    public Task<bool> ToggleClientStatus(int id)
    {
        var client = _unitOfWork.Client
            .GetAll()
            .FirstOrDefault(c => c.Id == id);

        if (client == null)
            return Task.FromResult(false);

        client.UserStatus = client.UserStatus == SD.UserStatusActive
            ? SD.UserStatusInactive
            : SD.UserStatusActive;

        _unitOfWork.Client.Update(client);
        _unitOfWork.Save();

        return Task.FromResult(true);
    }

    public Task DeleteClient(int id)
    {
        var client = _unitOfWork.Client
            .GetAll()
            .FirstOrDefault(c => c.Id == id);
        if (client != null)
        {
            _unitOfWork.Client.Remove(client);
            _unitOfWork.Save();

            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public Task<ClientEditVm> GetClientForEdit(int id)
    {
        var client = _unitOfWork.Client
            .GetAll(c => c.Id == id)
            .FirstOrDefault();

        return Task.FromResult(new ClientEditVm
        {
            Id = client.Id,
            FirstName = client.ApplicationUser.FirstName,
            LastName = client.ApplicationUser.LastName,
            Email = client.ApplicationUser.Email,
            Phone = client.ApplicationUser.PhoneNumber
        });
    }

    public Task<List<ClientItemVm>> GetClientsForExport(ClientFilter filter)
    {
        var query = _unitOfWork.Client.GetAll(c => true, "ApplicationUser");

        if (!string.IsNullOrEmpty(filter.SearchTerm))
            query = query.Where(c =>
                c.ApplicationUser.FirstName.Contains(filter.SearchTerm) ||
                c.ApplicationUser.LastName.Contains(filter.SearchTerm)
            );

        return Task.FromResult(query
            .Select(c => new ClientItemVm
            {
                Id = c.Id,
                FullName = $"{c.ApplicationUser.FirstName} {c.ApplicationUser.LastName}",
                Email = c.ApplicationUser.Email,
                RegistrationDate = c.ApplicationUser.CreatedAt,
                Status = c.UserStatus
            })
            .ToList());
    }
}
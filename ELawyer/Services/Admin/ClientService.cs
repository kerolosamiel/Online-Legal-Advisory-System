using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models.ViewModels.Admin.Client;
using ELawyer.Utility;
using Microsoft.AspNetCore.Identity;

namespace ELawyer.Services.Admin;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;

    public ClientService(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
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

        if (filter.IsActive.HasValue)
            query = query.Where(c =>
                c.UserStatus == (filter.IsActive.Value ? SD.UserStatusActive : SD.UserStatusInactive));

        var totalCount = query.Count();

        var clients = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ClientItemVm
            {
                Id = c.Id,
                FullName = $"{c.ApplicationUser.FirstName} {c.ApplicationUser.LastName}",
                Email = c.ApplicationUser.Email,
                Phone = c.ApplicationUser.PhoneNumber,
                RegistrationDate = c.ApplicationUser.CreatedAt,
                Status = c.UserStatus,
                IsActive = c.UserStatus == SD.UserStatusActive
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

    public Task<ClientDetailsVm> GetClientForDelete(int id)
    {
        var client = _unitOfWork.Client
            .GetAll(c => c.Id == id, "ApplicationUser")
            .FirstOrDefault();

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        return Task.FromResult(new ClientDetailsVm
        {
            Id = client.Id,
            FullName = $"{client.ApplicationUser.FirstName} {client.ApplicationUser.LastName}"
        });
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
        client.ApplicationUser.Email = client.ApplicationUser.Email;
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

    public Task<ClientEditVm> GetClientForEdit(int id)
    {
        var client = _unitOfWork.Client
            .GetAll(c => c.Id == id, "ApplicationUser")
            .FirstOrDefault();

        if (client == null)
            return Task.FromResult(new ClientEditVm());

        return Task.FromResult(new ClientEditVm
        {
            Id = client.Id,
            FirstName = client.ApplicationUser?.FirstName,
            LastName = client.ApplicationUser?.LastName,
            Email = client.ApplicationUser?.Email,
            Phone = client.ApplicationUser?.PhoneNumber
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

    public Task DeleteClient(ClientDetailsVm model)
    {
        var client = _unitOfWork.Client
            .GetAll()
            .FirstOrDefault(c => c.Id == model.Id);
        if (client != null)
        {
            _unitOfWork.Client.Remove(client);
            _unitOfWork.Save();

            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    private async Task<string> GenerateUniqueUsername(string firstName, string lastName)
    {
        var baseUsername = $"{firstName.ToLower()}_{lastName.ToLower()}".Replace(" ", "_");
        var username = baseUsername;
        var counter = 1;

        while (await _userManager.FindByNameAsync(username) != null)
        {
            username = $"{baseUsername}{counter}";
            counter++;
        }

        return username;
    }
}
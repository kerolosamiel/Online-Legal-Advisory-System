using ELawyer.Models.ViewModels.Admin.Client;

namespace ELawyer.Services.Admin;

public interface IClientService
{
    Task<ClientListVm> GetClientsWithPagination(ClientFilter filter, int page, int pageSize);
    Task<ClientDetailsVm> GetClientDetails(int id);
    Task<List<ClientItemVm>> GetClientsForExport(ClientFilter filter);
    Task<ClientEditVm> GetClientForEdit(int id);
    Task<int> CreateClient(ClientCreateVm model);
    Task<bool> UpdateClient(ClientEditVm model);
    Task<bool> ToggleClientStatus(int id);
    Task DeleteClient(int id);
}
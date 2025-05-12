namespace ELawyer.Models.ViewModels.Admin.Client;

public class ClientListVm
{
    public PaginatedList<ClientItemVm> Clients { get; set; }
    public ClientFilter? Filter { get; set; }
}
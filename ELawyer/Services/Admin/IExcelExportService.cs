using ELawyer.Models.ViewModels.Admin.Client;

namespace ELawyer.Services.Admin;

public interface IExcelExportService
{
    Stream ExportClients(List<ClientItemVm> clients);
}
namespace ELawyer.Models.ViewModels.Admin.Client;

public class ClientFilter
{
    public string? SearchTerm { get; set; }
    public DateTime? RegistrationDateFrom { get; set; }
    public DateTime? RegistrationDateTo { get; set; }
    public int? MinRequests { get; set; }
}
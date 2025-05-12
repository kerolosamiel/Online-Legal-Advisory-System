namespace ELawyer.Models.ViewModels.Admin.Client;

public class ClientItemVm
{
    public int? Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public string? Status { get; set; }
}
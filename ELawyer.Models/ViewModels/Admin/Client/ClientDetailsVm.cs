namespace ELawyer.Models.ViewModels.Admin.Client;

public class ClientDetailsVm
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public string? Status { get; set; }
    public int ActiveCases { get; set; }
    public string? Address { get; set; }
    public DateTime? LastUpdated { get; set; }
}
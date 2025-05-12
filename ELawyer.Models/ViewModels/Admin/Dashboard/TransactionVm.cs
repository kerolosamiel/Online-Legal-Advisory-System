namespace ELawyer.Models.ViewModels.Admin.Dashboard;

public class TransactionVm
{
    public string? ClientName { get; set; }
    public string? LawyerName { get; set; }
    public string? ServiceTitle { get; set; }
    public DateTime Date { get; set; }
    public DateTime? PayedAt { get; set; }
    public decimal Amount { get; set; }
}
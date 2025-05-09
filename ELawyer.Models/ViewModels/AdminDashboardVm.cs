using Microsoft.AspNetCore.Mvc.Rendering;

namespace ELawyer.Models.ViewModels;

public class AdminDashboardVm
{
    public int TotalClients { get; set; }
    public int TotalLawyers { get; set; }
    public int TotalOrders { get; set; }
    public double TotalEarnings { get; set; }
    public int PendingApprovals { get; set; }
    public decimal EarningsPercentageChange { get; set; }
    public bool EarningsIncreased { get; set; }
    public List<LawyerRegistrationVm> RecentLawyers { get; set; } = new();
    public List<TransactionVm> RecentTransactions { get; set; } = new();

    public int? SelectedClientId { get; set; }
    public int? SelectedLawyerId { get; set; }

    public SelectList? ClientList { get; set; }
    public SelectList? LawyerList { get; set; }
}
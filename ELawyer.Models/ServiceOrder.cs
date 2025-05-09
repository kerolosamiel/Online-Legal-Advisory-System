using System.ComponentModel.DataAnnotations.Schema;

namespace ELawyer.Models;

public class ServiceOrder
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public int ClientId { get; set; }
    public int LawyerId { get; set; }
    public int PaymentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ScheduledAt { get; set; }
    public double Amount { get; set; }

    [ForeignKey(nameof(ServiceId))] public virtual Service Service { get; set; } = new();
    [ForeignKey(nameof(PaymentId))] public virtual Payment Payment { get; set; } = new();
    [ForeignKey(nameof(ClientId))] public virtual Client Client { get; set; } = new();
    [ForeignKey(nameof(LawyerId))] public virtual Lawyer Lawyer { get; set; } = new();
}
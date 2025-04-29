namespace ELawyer.Models;

public class ServiceOrder
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public int ClientId { get; set; }
    public int PaymentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ScheduledAt { get; set; }
    public double Amount { get; set; }

    public virtual Service Service { get; set; } = new();
    public virtual Payment Payment { get; set; } = new();
}
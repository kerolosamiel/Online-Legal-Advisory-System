namespace ELawyer.Models;

public class Invoice
{
    public int Id { get; set; }
    public DateTime PaymentDate { get; set; }
    public double Amount { get; set; }
    public int ServiceOrderId { get; set; }
    public int ClientId { get; set; }
    public int PaymentId { get; set; }

    // Relationship
    public virtual Client Client { get; set; } = new();
}
using System.ComponentModel.DataAnnotations.Schema;

namespace ELawyer.Models;

public class Payment
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? Recievedat { get; set; }
    public int LawyerId { get; set; }
    public int ClientId { get; set; }

    // Relationship
    [ForeignKey("ClientID")] public virtual Client? Client { get; set; } = new();
    [ForeignKey("LawyerID")] public virtual Lawyer Lawyer { get; set; } = new();
    public virtual ServiceOrder ServiceOrder { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELawyer.Models;

public class Consultation
{
    public int Id { get; set; }
    [MaxLength(100)] public string? Title { get; set; }
    [MaxLength(400)] public string? Description { get; set; }
    [MaxLength(int.MaxValue)] public string? Attachments { get; set; }
    public int? LawyerId { get; set; }
    public int? ClientId { get; set; }
    public int? PaymentId { get; set; }

    // Relationship
    [ForeignKey(nameof(LawyerId))] public virtual Lawyer Lawyer { get; set; } = new();
    [ForeignKey(nameof(ClientId))] public virtual Client Client { get; set; } = new();
}
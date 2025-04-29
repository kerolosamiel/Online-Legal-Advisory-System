using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELawyer.Models;

public class Response
{
    public int Id { get; set; }
    [Required] [MaxLength(100)] public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(5, MinimumLength = 400)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(int.MaxValue)] public string? Attachments { get; set; }
    public int? ClientId { get; set; }
    public int? LawyerId { get; set; }
    public int? ConsultationId { get; set; }

    // RelationShip
    [ForeignKey(nameof(LawyerId))] public Lawyer? Lawyer { get; set; }
    [ForeignKey(nameof(ClientId))] public Client? Client { get; set; }
}
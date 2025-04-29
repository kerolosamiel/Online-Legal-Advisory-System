using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELawyer.Models;

public class Rating
{
    [Key] public int ID { get; set; }
    [MaxLength(400)] public string? Comment { get; set; }
    [Required] [Range(0, 5)] public int Rate { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? LawyerId { get; set; }
    public int? ClientId { get; set; }

    // Relationship
    [ForeignKey(nameof(ClientId))] public Client Client { get; set; } = new();
    [ForeignKey(nameof(LawyerId))] public Lawyer Lawyer { get; set; } = new();
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELawyer.Models;

public enum CType
{
    Individual,
    Company
}

public class Client
{
    [Key] public int Id { get; set; }
    [StringLength(200, MinimumLength = 2)] public string? ImageUrl { get; set; }
    [MaxLength(100)] public string? Address { get; set; }
    public int? NoOfLawyers { get; set; }

    public CType ClientType { get; set; }

    [MaxLength(10)] public string? UserStatus { get; set; }
    public int? ClientRatingId { get; set; }

    public string? UserId { get; set; }


    // Relationship
    [ForeignKey(nameof(UserId))] public virtual ApplicationUser ApplicationUser { get; set; }
    public virtual ICollection<Consultation> Consultations { get; set; } = new HashSet<Consultation>();
    [ForeignKey(nameof(ClientRatingId))] public ICollection<Rating>? Rating { get; set; } = new HashSet<Rating>();
}
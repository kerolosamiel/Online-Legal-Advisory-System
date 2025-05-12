using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELawyer.Models;

public class Admin
{
    public int Id { get; set; }
    [StringLength(200, MinimumLength = 2)] public string? ImageUrl { get; set; }
    [MaxLength(100)] public string? Address { get; set; }
    public string? UserId { get; set; }

    // Relationship
    [ForeignKey(nameof(UserId))] public virtual ApplicationUser? ApplicationUser { get; set; } = new();
}
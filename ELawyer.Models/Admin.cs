using System.ComponentModel.DataAnnotations;

namespace ELawyer.Models;

public class Admin
{
    public int Id { get; set; }
    [StringLength(200, MinimumLength = 2)] public string? ImageUrl { get; set; }
    [MaxLength(100)] public string? Address { get; set; }

    // Relationship
    public virtual ApplicationUser ApplicationUser { get; set; } = new();
}
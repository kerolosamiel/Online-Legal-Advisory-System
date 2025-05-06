using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ELawyer.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(25)] [Required] public string FirstName { get; set; } = "";

    [MaxLength(25)] [Required] public string LastName { get; set; } = "";

    // Remove email-related validation
    public override string? Email { get; set; }
    [MaxLength(50)] public string? Role { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastLogin { get; set; }
    public int? ClientId { get; set; }
    public int? LawyerId { get; set; }
    public int? AdminId { get; set; }

    [ForeignKey(nameof(AdminId))] public virtual Admin Admin { get; set; }
    [ForeignKey(nameof(ClientId))] public virtual Client Client { get; set; }
    [ForeignKey(nameof(LawyerId))] public virtual Lawyer Lawyer { get; set; }
}
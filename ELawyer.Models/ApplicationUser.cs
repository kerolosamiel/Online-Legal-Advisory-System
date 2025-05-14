using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ELawyer.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(25)] [Required] public string? FirstName { get; set; } = "";

    [MaxLength(25)] [Required] public string? LastName { get; set; } = "";

    [MaxLength(50)] public string? Role { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastLogin { get; set; }

    public virtual Admin? Admin { get; set; }
    public virtual Client? Client { get; set; }
    public virtual Lawyer? Lawyer { get; set; }
}
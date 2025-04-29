using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELawyer.Models;

public enum ServiceTypes
{
    Consultation,
    LegalService
}

public enum ServiceStatus
{
    Available,
    Inactive
}

public class Service
{
    [Key] public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 10)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 50)]
    public string Description { get; set; } = string.Empty;

    public ServiceTypes ServiceType { get; set; }
    public ServiceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public TimeOnly Duration { get; set; }
    public int LawyerId { get; set; }

    // relationship
    [ForeignKey(nameof(LawyerId))] public virtual Lawyer? Lawyer { get; set; }
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new HashSet<ServiceOrder>();
}
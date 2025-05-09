using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELawyer.Models;

public class Lawyer
{
    [Key] public int Id { get; set; }
    [StringLength(200, MinimumLength = 5)] public string? ImageUrl { get; set; }
    [MaxLength(400)] public string? About { get; set; }
    [MaxLength(100)] public string? Address { get; set; }
    public int? NoOfClients { get; set; }
    [MaxLength(int.MaxValue)] public string? FrontCardImage { get; set; }
    [MaxLength(int.MaxValue)] public string? BackCardImage { get; set; }
    [MaxLength(10)] public string? UserStatus { get; set; }

    [MaxLength(35)]
    [Required(ErrorMessage = "License number is required.")]
    public string LinceseNumber { get; set; } = string.Empty;

    public int? ExperienceYears { get; set; }

    [MaxLength(500)] public string? LinkedIn { get; set; }

    public int? ConsultationFee { get; set; }
    public double? AverageRateing { get; set; }
    public int? ServiceId { get; set; }
    public int? LawyerRatingId { get; set; }
    public string? UserId { get; set; }


    // Realationship
    public virtual ICollection<LawyerSpecialization> LawyerSpecializations { get; set; } =
        new HashSet<LawyerSpecialization>();

    [ForeignKey(nameof(UserId))] public virtual ApplicationUser ApplicationUser { get; set; }
    [ForeignKey(nameof(ServiceId))] public virtual ICollection<Service> Services { get; set; } = new HashSet<Service>();

    [ForeignKey(nameof(LawyerRatingId))]
    public virtual ICollection<Rating> Rating { get; set; } = new HashSet<Rating>();
}
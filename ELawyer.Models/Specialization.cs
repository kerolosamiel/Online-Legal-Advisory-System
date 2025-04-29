using System.ComponentModel.DataAnnotations;

namespace ELawyer.Models;

public class Specialization
{
    [Key] public int ID { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(400, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<SubSpecialization> SubSpecializations { get; set; } = new HashSet<SubSpecialization>();

    public virtual ICollection<LawyerSpecialization> LawyerSpecializations { get; set; } =
        new HashSet<LawyerSpecialization>();
}
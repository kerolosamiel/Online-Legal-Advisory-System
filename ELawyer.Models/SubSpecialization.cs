using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELawyer.Models;

public class SubSpecialization
{
    [Key] public int ID { get; set; }

    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;

    public int SpecializationID { get; set; }

    [ForeignKey(nameof(SpecializationID))] public virtual Specialization Specialization { get; set; }
}
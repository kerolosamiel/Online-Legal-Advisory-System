using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELawyer.Models;

public class SubSpecialization
{
    [Key] public int ID { get; set; }

    [Required] public string Name { get; set; }

    public int SpecializationID { get; set; }

    [ForeignKey(nameof(SpecializationID))] public virtual Specialization Specialization { get; set; }
}
namespace ELawyer.Models;

public class LawyerSpecialization
{
    public int LawyerId { get; set; }
    public int SpecializationId { get; set; }

    // Relationship
    public virtual Specialization Specialization { get; set; } = new();
    public virtual Lawyer Lawyer { get; set; } = new();
}
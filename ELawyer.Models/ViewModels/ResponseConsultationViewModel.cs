namespace ELawyer.Models.ViewModels;

public class ResponseConsultationViewModel
{
    public int Id { get; set; }
    public Consultation Consultation { get; set; } = new();

    public Response Response { get; set; } = new();
}
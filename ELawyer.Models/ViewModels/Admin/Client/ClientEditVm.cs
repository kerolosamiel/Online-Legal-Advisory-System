namespace ELawyer.Models.ViewModels.Admin.Client;

public class ClientEditVm : ClientCreateVm
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}
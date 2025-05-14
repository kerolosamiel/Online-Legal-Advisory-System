using System.ComponentModel.DataAnnotations;

namespace ELawyer.Models.ViewModels.Admin.Client;

public class ClientCreateVm
{
    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    [EmailAddress] public string Email { get; set; }

    [Phone] public string? Phone { get; set; }
}
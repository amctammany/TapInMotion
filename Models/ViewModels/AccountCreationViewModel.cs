using System.ComponentModel.DataAnnotations;

namespace TapInMotion.Models.ViewModels;



public class AccountCreationViewModel
{
    [Required(ErrorMessage = "Please enter a username.")]
    public string? Name { get; set; }
    public string? Email { get; set; }

    public AccountType AccountType { get; set; }
    public int SchoolID { get; set; }
    public int? StudentID { get; set; }
    public int? AdministratorID { get; set; }

    [Required]
    [UIHint("password")]
    public string? Password { get; set; }
    public string? ReturnUrl { get; set; } = "/";
}

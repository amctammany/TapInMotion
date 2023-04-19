using System.ComponentModel.DataAnnotations;

namespace TapInMotion.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please enter a username.")]
    public string? Email { get; set; }

    [Required]
    [UIHint("password")]
    public string? Password { get; set; }
    public string? ReturnUrl { get; set; } = "/";
}

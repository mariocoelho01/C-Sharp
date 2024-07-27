using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Put your E-mail")]
    [EmailAddress(ErrorMessage = "E-mail is invalid!")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Put your password")]
    public string Password { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class RegisterViewModel
{
    [Required (ErrorMessage = "Name is required")]
    public string Name { get; set; }

    
    [Required (ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "E-mail is invalid")]
    public string Email { get; set; }

    /*[Required(ErrorMessage = "Very short password")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    public string Password { get; set; }*/
}
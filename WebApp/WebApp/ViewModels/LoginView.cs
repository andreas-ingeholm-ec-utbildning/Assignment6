using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class LoginView
{

    [Required(ErrorMessage = "You must enter a email address.")]
    [Display(Name = "Email address"), DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; } = null!;

    [Required(ErrorMessage = "You must enter a password.")]
    [Display(Name = "Password"), DataType(DataType.Password)]
    public string Password { get; set; } = null!;

}
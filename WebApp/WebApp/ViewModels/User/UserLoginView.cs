using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class UserLoginView
{

    [Display(Name = "Email address"), DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Password"), DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

}

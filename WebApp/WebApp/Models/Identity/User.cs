using Microsoft.AspNetCore.Identity;
using WebApp.ViewModels;

namespace WebApp.Models.Identity;

public class User : IdentityUser<Guid>
{

    [ProtectedPersonalData] public string FirstName { get; set; } = null!;
    [ProtectedPersonalData] public string LastName { get; set; } = null!;

    public static implicit operator User(UserRegisterView view) =>
        new()
        {
            UserName = view.Email,
            Email = view.Email,
            FirstName = view.FirstName,
            LastName = view.LastName,
            PhoneNumber = view.PhoneNumber,
        };

}

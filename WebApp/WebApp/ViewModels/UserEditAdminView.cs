using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class UserEditAdminView : UserEditView
{

    //Set from AdminController
    public bool IsLoggedIn { get; set; }

    //For display
    public string? Email { get; set; }

    public Guid ID { get; set; }
    public string? Role { get; set; }

    public static implicit operator UserEditAdminView(UserProfileEntity user) =>
        new()
        {
            ID = user.UserID,
            Email = user.User.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            StreetName = user.StreetName,
            PostalCode = user.PostalCode,
            City = user.City,
        };

}
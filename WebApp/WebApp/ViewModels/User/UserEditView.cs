using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class UserEditView
{

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? StreetName { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }

    public static implicit operator UserEditView(UserProfileEntity user) =>
        new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            StreetName = user.StreetName,
            PostalCode = user.PostalCode,
            City = user.City,
        };

}

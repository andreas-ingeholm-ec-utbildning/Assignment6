using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class UserEditView
{

    public Guid? ID { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? StreetName { get; set; } = null!;
    public string? PostalCode { get; set; } = null!;
    public string? City { get; set; } = null!;

    public static implicit operator UserEditView(UserProfileEntity user) =>
        new()
        {
            ID = user.UserID,
            FirstName = user.FirstName,
            LastName = user.LastName,
            StreetName = user.StreetName,
            PostalCode = user.PostalCode,
            City = user.City,
        };

}
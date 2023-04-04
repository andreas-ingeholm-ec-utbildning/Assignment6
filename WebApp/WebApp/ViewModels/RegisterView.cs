using System.ComponentModel.DataAnnotations;
using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class RegisterView
{

    [Display(Name = "Email address"), DataType(DataType.EmailAddress)]
    [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])")] //https://stackoverflow.com/a/201378
    public string EmailAddress { get; set; } = null!;

    [Display(Name = "First name")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last name")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Password"), DataType(DataType.Password)]
    [RegularExpression(
        pattern: @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", //https://stackoverflow.com/a/21456918
        ErrorMessage = "Password must be at least 8 characters long, and must contain:\n\n* 1 lowercase character\n* 1 uppercase character\n* 1 number character\n* 1 special character")]
    public string Password { get; set; } = null!;

    [Display(Name = "Confirm password"), DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

    [Display(Name = "Street name")]
    public string? StreetName { get; set; } = null!;

    [Display(Name = "Postal code")]
    public string? PostalCode { get; set; } = null!;

    [Display(Name = "City")]
    public string? City { get; set; } = null!;

    public static implicit operator UserEntity(RegisterView data)
    {
        var user = new UserEntity { EmailAddress = data.EmailAddress };
        user.GeneratePassword(data.Password);
        return user;
    }

    public static implicit operator ProfileEntity(RegisterView data) =>
        new()
        {
            FirstName = data.FirstName,
            LastName = data.LastName,
            StreetName = data.StreetName,
            PostalCode = data.PostalCode,
            City = data.City
        };

}

using System.ComponentModel.DataAnnotations;
using WebApp.Models.Entities;
using WebApp.Models.Identity;

namespace WebApp.ViewModels;

public class UserRegisterView
{

    [Display(Name = "First name")]
    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last name")]
    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email address"), DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Password"), DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(
        pattern: @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", //https://stackoverflow.com/a/21456918
        ErrorMessage = "Password must be at least 8 characters long, and must contain:\n\n* 1 lowercase character\n* 1 uppercase character\n* 1 number character\n* 1 special character")]
    public string Password { get; set; } = null!;

    [Display(Name = "Confirm password"), DataType(DataType.Password)]
    [Required(ErrorMessage = "Passwords do not match.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

    [Display(Name = "Street name")]
    public string? StreetName { get; set; }

    [Display(Name = "Postal code")]
    public string? PostalCode { get; set; }

    [Display(Name = "City")]
    public string? City { get; set; }

    public static implicit operator User(UserRegisterView view) =>
        new()
        {
            UserName = view.Email,
            PhoneNumber = view.PhoneNumber,
            Email = view.Email,
        };

    public static implicit operator UserProfileEntity(UserRegisterView view) =>
        new()
        {
            FirstName = view.FirstName,
            LastName = view.LastName,
            StreetName = view.StreetName,
            PostalCode = view.PostalCode,
            City = view.City,
        };

}

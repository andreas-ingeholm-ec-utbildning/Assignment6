using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels;

public class ContactFormView
{

    [Display(Name = "Your name *")]
    public string Name { get; set; } = null!;

    [Display(Name = "Your email address *")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string EmailAddress { get; set; } = null!;

    [Display(Name = "Phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }
    public string? Company { get; set; }

    [Display(Name = "Message *")]
    [MinLength(3)]
    public string Message { get; set; } = null!;

    public static implicit operator ContactData(ContactFormView view) =>
        new()
        {
            Name = view.Name,
            EmailAddress = view.EmailAddress,
            PhoneNumber = view.PhoneNumber,
            Company = view.Company,
            Message = view.Message,
        };

}
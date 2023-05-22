using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class ContactData
{

    public Guid ID { get; set; }
    public string Name { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;

    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }
    public string? Company { get; set; }
    public string Message { get; set; } = null!;

}
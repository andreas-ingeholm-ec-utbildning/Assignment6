using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Models.Identity;

namespace WebApp.Models.Entities;

public class UserProfileEntity
{

    [Key, ForeignKey(nameof(User))]
    public Guid UserID { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? StreetName { get; set; } = null!;
    public string? PostalCode { get; set; } = null!;
    public string? City { get; set; } = null!;

    public User User { get; set; } = null!;

}

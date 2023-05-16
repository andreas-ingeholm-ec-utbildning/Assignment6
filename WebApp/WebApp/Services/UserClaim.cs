using System.Security.Claims;

namespace WebApp.Services;

/// <summary>Specifies user claims.</summary>
public static class UserClaim
{

    /// <summary>Specifies the display name of a user.</summary>
    public const string DisplayName = "DisplayName";

    /// <summary>Specfies the role of a user.</summary>
    public const string Role = ClaimTypes.Role;

}

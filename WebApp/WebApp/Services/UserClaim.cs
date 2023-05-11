using System.Security.Claims;

namespace WebApp.Services;

public static class UserClaim
{
    public const string DisplayName = "DisplayName";
    public const string Role = ClaimTypes.Role;
}

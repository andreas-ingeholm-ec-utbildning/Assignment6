namespace WebApp.Services;

/// <summary>Specifies user roles.</summary>
public static class UserRole
{

    /// <summary>Specifies that the user is a admin.</summary>
    public const string Admin = "admin";

    /// <summary>Specifies that the user is regular user.</summary>
    public const string User = "user";

    /// <summary>Enumerates all user roles.</summary>
    public static string[] Enumerate() =>
        new string[] { Admin, User };

}

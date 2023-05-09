namespace WebApp.Services;

public static class UserRole
{

    public const string Admin = "admin";
    public const string User = "user";
    public const string Any = "admin,user";

    public static string[] Enumerate() =>
        new string[] { Admin, User };

}

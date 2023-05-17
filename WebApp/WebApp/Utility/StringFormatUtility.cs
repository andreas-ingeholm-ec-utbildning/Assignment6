namespace WebApp.Services;

public static class StringFormatUtility
{

    public static string ToMoneyString(this decimal value) =>
        string.Format("{0:C}", value);

}

using System.Text.RegularExpressions;

public static class StringIsUrlHelper
{
    public static bool IsUrl(this  string url)
    {
        string pattern = @"^https?:\/\/[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(:[0-9]+)?(\/.*)?$";
        return Regex.IsMatch(url, pattern);
    }
}
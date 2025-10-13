using System.Text.RegularExpressions;

namespace FoxyWebAppManager.Extensions
{
    public static class StringExtensions
    {
        extension(string s)
        {
            public bool IsUrl()
            {
                string pattern = @"^https?:\/\/[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(:[0-9]+)?(\/.*)?$";
                return Regex.IsMatch(s, pattern);
            }
        }
    }
}

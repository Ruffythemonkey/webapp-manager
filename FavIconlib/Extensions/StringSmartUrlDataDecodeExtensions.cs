using System.Net;

namespace FavIconlib.Extensions
{
    internal static class StringSmartUrlDataDecodeExtensions
    {
        extension(string input) 
        {
            public string SmartDecode()
            {
                if (string.IsNullOrEmpty(input))
                    return input;

                string result = input;
                bool changed;

                do
                {
                    changed = false;

                    // 1) URL-Encoding erkennen und decodieren
                    if (ContainsUrlEncoding(result))
                    {
                        string unescaped = Uri.UnescapeDataString(result);
                        if (unescaped != result)
                        {
                            result = unescaped;
                            changed = true;
                        }
                    }

                    // 2) HTML-Encoding erkennen und decodieren
                    if (ContainsHtmlEntities(result))
                    {
                        string decoded = WebUtility.HtmlDecode(result);
                        if (decoded != result)
                        {
                            result = decoded;
                            changed = true;
                        }
                    }

                } while (changed);

                return result;
            }

            private static bool ContainsUrlEncoding(string s)
            {
                // erkennt %20, %3C, %2F ... 
                for (int i = 0; i < s.Length - 2; i++)
                {
                    if (s[i] == '%' &&
                        IsHexDigit(s[i + 1]) &&
                        IsHexDigit(s[i + 2]))
                    {
                        return true;
                    }
                }
                return false;
            }

            private static bool IsHexDigit(char c)
            {
                return ("0123456789ABCDEFabcdef".IndexOf(c) >= 0);
            }

            private static bool ContainsHtmlEntities(string s)
            {
                // einfache, aber effektive Erkennung
                return s.Contains("&lt;") ||
                       s.Contains("&gt;") ||
                       s.Contains("&amp;") ||
                       s.Contains("&quot;") ||
                       s.Contains("&#");
            }

        }
    }
}

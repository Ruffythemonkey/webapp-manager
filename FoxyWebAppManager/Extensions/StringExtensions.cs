using FoxyWebAppManager.Helpers;
using System.Net;
using System.Text.RegularExpressions;

namespace FoxyWebAppManager.Extensions
{
    public static class StringExtensions
    {
        private readonly static SemaphoreSlim _slim = new(1);


        extension(string s)
        {
            public bool IsUrl()
            {
                //return Uri.TryCreate(s, UriKind.Absolute, out var uri)
                //    && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
                string pattern = @"^https?:\/\/[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(:[0-9]+)?(\/.*)?$";
                return Regex.IsMatch(s, pattern);
            }

            public bool IsIp()
                => IPAddress.TryParse(s, out _);

            public bool IsUrlOrIp()
            {
                if (!Uri.TryCreate(s, UriKind.Absolute, out var uri))
                    return false;

                if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                    return false;

                // Host ist Domain oder IP?
                return Uri.CheckHostName(uri.Host) != UriHostNameType.Unknown;
            }


            public async Task<bool> CompleteCheckUpUrl()
            {
                if (s.IsUrlOrIp() || s.IsIp())
                {
                    try
                    {
                        await _slim.WaitAsync();
                        if (await s.IsDnsResolvableUrlAsync())
                        {
                       
                            using HttpClient client = new();
                            client.Timeout = TimeSpan.FromSeconds(2);
                            var req = await client.GetAsync(s,HttpCompletionOption.ResponseHeadersRead);
                            return true;
                        }
                    }
                    catch (Exception)
                    {

                        return false;
                    }
                    finally
                    {
                        _slim.Release();
                    }
                }

                return false;
            }

            public string ToUriSchemeString()
            {
                if (!s.StartsWith("https://") && !s.StartsWith("http://"))
                {
                    return $"https://{s}";
                }
                return s;
            }
        }
    }
}

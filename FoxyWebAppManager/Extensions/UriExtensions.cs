using System.Threading.Tasks;

namespace FoxyWebAppManager.Extensions
{
    public static class UriExtension
    {
        extension(Uri uri)
        {
            /// <summary>
            /// bsp input https://www.escsoft.de
            /// </summary>
            /// <returns>escsoft</returns>
            public string GetDomainNameWithoutExtension()
                => Task.Run(() => TLDExtractor.TLDExtractor.Extract(uri)).GetAwaiter().GetResult().Domain;


            /// <summary>
            /// bsp input https://www.escsoft.de
            /// </summary>
            /// <returns>escsoft.de</returns>
            public string GetTopLevelDomain()
               => $"{uri.Scheme}://{Task.Run(()=> TLDExtractor.TLDExtractor.Extract(uri)).GetAwaiter().GetResult().EffectiveDomain}";

            public bool DomainHasSubLevelDomain()
                => uri.DnsSafeHost.Split(".").Count() > 1;

            /// <summary>
            /// Check is the uri moved HTTP Code 301 and give the right 200 URL
            /// </summary>
            /// <returns></returns>
            public async Task<Uri> UriMovedCheckup()
            {
                try
                {
                    using HttpClient client = new HttpClient();
                    var req = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                    req.EnsureSuccessStatusCode();
                    return (req.RequestMessage!.RequestUri!);
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }
    }

}
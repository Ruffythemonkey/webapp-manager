using FavIconlib.AbstractClasses;
using FavIconlib.Extensions;
using HtmlAgilityPack;

namespace FavIconlib.Utils
{
    internal class FavIconFetcher : Web
    {
        private readonly string _gstring = "https://www.google.com/s2/favicons?domain={0}&sz={1}";
        public int IconSize { get; set; } = 64;

        /// <summary>
        /// FavIco from google
        /// </summary>
        /// <param name="url"></param>
        /// <param name="prioSize"></param>
        /// <returns>HttpResponseMessage Content byte[]</returns>
        private async Task<HttpResponseMessage> FavIconFetchGoogle(string url)
            => await GetAsync(string.Format(_gstring, [url, IconSize]));

        public async Task<string> FavIconFetchSelf(string url)
        {
            var req = await GetAsync(url);
            req.EnsureSuccessStatusCode();
            var html = await req.Content.ReadAsStringAsync();
            var ret = new Uri(new Uri(url), HtmlFavIconFetch(html).href).ToString();
            return ret;
        }


        private Models.FavIcon HtmlFavIconFetch(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            return doc
                .DocumentNode
                .SelectNodes("//head/link[@rel]")
                .Where(node => node.GetAttributeValue("rel", "")
                .ToLower().Contains("icon")
                && !string.IsNullOrEmpty(node.GetAttributeValue("href", "")))
                //select all rel icon nodes with href
                .Select(node =>
                {
                    return new Models.FavIcon()
                    {
                        href = node.GetAttributeValue("href", ""),
                        sizes = node.GetAttributeValue("sizes", "").SizeToInt(),
                        type = node.GetAttributeValue("type", "")
                    };
                })
                .OrderByDescending( n=> n.sizes /*Math.Abs(n.sizes - IconSize)*/)
                .ToList()
                .First();
        }


        private string CreateFileName(Uri uri, out string filename)
        {
            var iconname = $"{uri.Scheme}://{uri.DnsSafeHost}";
            var tempfn = uri.GetHashCode().ToString().Replace("-", "");
            filename = Path.Combine(Path.GetTempPath(), $"{tempfn}.ico");
            return iconname;
        }

        private async void CreateIcon(byte[] content, string file)
        {
            await File.WriteAllBytesAsync($"{file}.png", content);
            FavIconlib.Helper.ImageIconConverter.ConvertToIcon($"{file}.png", file, IconSize);
            File.Delete($"{file}.png");
        }

    }
}

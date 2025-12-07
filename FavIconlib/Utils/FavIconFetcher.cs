using FavIconlib.AbstractClasses;
using FavIconlib.Extensions;
using FavIconlib.Helper;
using HtmlAgilityPack;
using System.Web;

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

        public async Task<Stream> FavIconFetchSelf(string url, int size)
            => await IconStream(url, size);

        private async Task<(string requesturl,HtmlDocument doc)> DocumentAsync(string url)
        {
            var req = await GetAsync(url);
            var res = await req.Content.ReadAsStreamAsync();
            var doc = new HtmlDocument();
            doc.Load(res);
            return (requesturl: req.RequestMessage.RequestUri.ToString(),doc: doc);
        }


        private bool nodeCheck(HtmlNode node)
        {
            //is <meta content=""/>
            var metaContent = node.GetAttributeValue("content", "");
            //extension of meta content="" eg .png 
            var metaContentExt = Path.GetExtension(metaContent);

            return (node.GetAttributeValue("rel", "").ToLower().Contains("icon")
                && !string.IsNullOrEmpty(node.GetAttributeValue("href", "")))
                ||
                (
                    //Check is Format is compatible
                    Models.FavIcon.Formats.Any(x => metaContentExt == x)
                    //TODO: Check Size from Image
                    ||
                    //metadata = base64 Data? Format data is unkown!
                    metaContent.StartsWith("data:")
                );
        }

        private Models.FavIcon nodeSelectData(HtmlNode node)
        {
            var href = node.GetAttributeValue("href", "");
            return new Models.FavIcon()
            {
                href = href == string.Empty ? node.GetAttributeValue("content", "") : href,
                sizes = node.GetAttributeValue("sizes", "").SizeStringToInt()
            };
        }

        private async Task<List<Models.FavIcon>> HtmlFavIconFetch(string url)
        {
            var doc = await DocumentAsync(url);
            //doc.requesturl = 301 enhancer
            url = doc.requesturl;
            return doc.doc
           .DocumentNode
            //represent <head> <link rel/> <meta content/> </head>
            .SelectNodes("//head/link[@rel] | //head/meta[@content]")
            //all icon href urls
            .Where(nodeCheck)
            //select all rel icon nodes with href
            .Select(nodeSelectData)
            //check is type determined
            .Where(x=> x.type != "none")
            //Set the url string to the complete collection
            .SetUrl(url)
            ////order size when available
            //.OrderByDescending(n => n.sizes)
            .ToList();
        }

        private async Task<Models.FavIcon> BestQualitySeperator(string url, int size)
        {
            var resultsHtml = await HtmlFavIconFetch(url);
            var resultsImages = (await resultsHtml.SetUnknowSizesData())
                .OrderByDescending(x => x.sizes)
                .ToList();
            if (resultsImages.Count == 0)
            {
                throw new Exceptions.FavIconlibException("website has no favicon header");
            }

            return resultsImages.FirstOrDefault(x => x.sizes >= size)
                ?? resultsImages.FirstOrDefault(x => x.type == ".svg")
                ?? resultsImages.First();
        }

        private async Task<Stream> IconStream(string url, int size)
        {
            var result = await BestQualitySeperator(url, size);

            if (result.type == ".svg")
            {
                //TODO: this can svg but it can png ico
                if (result.IsHrefDirectData is byte[] bytedata) //when href has direct data
                {
                    var svgstream = SvgToPngByteStream.ConvertSvgToPng(new Models.Svg(bytedata), size);
                    return ImageStreamToIcon.ConvertImageToIcon(svgstream);
                }

                var svgreq = await GetAsync(result.IconUrl!.ToString());
                svgreq.EnsureSuccessStatusCode();
                var svgresult = await svgreq.Content.ReadAsStringAsync();
                var svg = SvgToPngByteStream.ConvertSvgToPng(new Models.Svg(svgresult), size);
                return ImageStreamToIcon.ConvertImageToIcon(svg);
            }
            else //is not svg
            {
                if (result.IsHrefDirectData is byte[] bytedata)
                {
                    using MemoryStream stream = new MemoryStream(bytedata);
                    return ImageStreamToIcon.ConvertImageToIcon(stream);
                }

                var req = await GetAsync(result.IconUrl!.ToString());
                req.EnsureSuccessStatusCode();
                var res = await req.Content.ReadAsStreamAsync();


                return ImageStreamToIcon.ConvertImageToIcon(res);
            }
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

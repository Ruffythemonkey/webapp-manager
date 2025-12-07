using FavIconlib.Exceptions;
using SkiaSharp;
using Svg.Skia;
using System.Text;
using System.Web;

namespace FavIconlib.Models
{
    public class FavIcon
    {
        /// <summary>
        /// supportet formats
        /// </summary>
        public static readonly List<string> Formats = [".png", ".svg", ".ico"];

        public string url
        {
            get;
            set => field = Uri.TryCreate(value, UriKind.Absolute, out _)
                ? value : throw new FavIconlibConvertException($"{value} is not an confirm url");
        } = string.Empty;
        public string href { get; set; } = "";
        public uint sizes { get; set; }
        
        /// <summary>
        /// Gets image type, if none then type cannot determint
        /// </summary>
        public string type
        {
            get => string.IsNullOrEmpty(field) ? TypeSelector(field) : field;
        } = string.Empty;

        public byte[]? IsHrefDirectData
        {
            get {
                if (field is byte[])
                    return field;

                if (href.StartsWith("data:"))
                {
                    var str = href.Split(",");

                    if (str.First().Contains("base64"))
                        field = Convert.FromBase64String(str[1]);

                    field = field ?? System.Text.Encoding.UTF8.GetBytes(HttpUtility.HtmlDecode(str[1]));
                }
                return field;
            }
        }

        private string TypeSelector(string field)
        {
            if (IsHrefDirectData is byte[] data)
            {
                if (Encoding.UTF8.GetString(data).Contains("<svg", StringComparison.OrdinalIgnoreCase))
                {
                    field = ".svg";
                    return field;
                }

                using SKCodec codec = SKCodec.Create(new SKMemoryStream(data));
                if ( codec !=null)
                {
                    field = codec.EncodedFormat switch
                    {
                        SKEncodedImageFormat.Png => ".png",
                        SKEncodedImageFormat.Ico => ".ico",
                        _ => ".png"
                    };
                    this.sizes = (uint)Math.Max(0,codec.Info.Width);
                    return field;
                }

                field = "none";
                return field;
            }

            var ext = Path.GetExtension(href);

            field = href switch
            {
                var s when s.Contains("image/png") => ".png",
                var s when s.Contains("image/svg") => ".svg",
                var s when s.Contains("image/ico") => ".ico",
                var s when s.Contains("image/vnd.microsoft.icon") => ".ico",
                _ => string.IsNullOrEmpty(ext) ? "none" : ext
            };

            return field;
        }


        public Uri? IconUrl { get => IsHrefDirectData == null 
                && !string.IsNullOrEmpty(href) 
                && !string.IsNullOrEmpty(url)
                ? new Uri(new Uri(url), href) : null; }

    }
}

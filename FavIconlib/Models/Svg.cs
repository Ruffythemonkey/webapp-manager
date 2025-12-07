using HtmlAgilityPack;
using FavIconlib.Extensions;
using System.Text;

namespace FavIconlib.Models
{
    internal class Svg
    {
        public Svg(string svgstring)
            => SvgString = svgstring;
        public Svg(byte[] svgbytes)
            => SvgString = Encoding.UTF8.GetString(svgbytes);
        public Svg() { }
        public string SvgString { get => field.SmartDecode() ; set; } = string.Empty;
    }
}

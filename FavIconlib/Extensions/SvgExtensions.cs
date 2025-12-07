using System.Text.RegularExpressions;

namespace FavIconlib.Extensions
{
    internal static class SvgExtensions
    {
        extension(Models.Svg svg)
        {
            public string NormalizeSvg()
            {
                var ret = svg.SvgString;
                // 1. Entferne display-p3 Farbdefinitionen
                ret = Regex.Replace(ret, @"fill:color\(display-p3[^\)]*\)", "", RegexOptions.IgnoreCase);

                //// 2. Entferne alle @media Regeln
                //ret = Regex.Replace(ret, @"@media[^{]+\{[^}]+\}", "", RegexOptions.Singleline);

                //// 3. Entferne externe Stylesheet-Imports
                //ret = Regex.Replace(ret, @"@import[^;]+;", "", RegexOptions.Singleline);

                //// 4. Entferne <foreignObject>
                //ret = Regex.Replace(ret, @"<foreignObject[\s\S]*?</foreignObject>", "", RegexOptions.IgnoreCase);

                //// 5. Entferne Filter, die Skia inkompatibel sind
                //ret = Regex.Replace(ret, @"<filter[\s\S]*?</filter>", "", RegexOptions.IgnoreCase);

                // 6. Entferne ClipPaths nur wenn nötig (Optional)
                // svg = Regex.Replace(svg, "<clipPath[\\s\\S]*?</clipPath>", "");

                // 7. double semicolons aufräumen
                ret = ret.Replace(";;", ";");

                return ret;
            }
        }
    }
}

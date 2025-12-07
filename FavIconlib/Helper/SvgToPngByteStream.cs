using FavIconlib.Extensions;
using SkiaSharp;
using Svg.Skia;
using System.Text;

namespace FavIconlib.Helper
{
    internal static class SvgToPngByteStream
    {
        public static Stream ConvertSvgToPng(Models.Svg svg, int size)
        {
            try
            {
                Stream ret = new MemoryStream();
                var nSvg = svg.NormalizeSvg();
                var svgDocument = new SKSvg();
                svgDocument.Load(new MemoryStream(Encoding.UTF8.GetBytes(nSvg)));

                using (var bitmap = new SKBitmap(size, size))
                using (var canvas = new SKCanvas(bitmap))
                {
                    canvas.Clear(SKColors.Transparent);

                    var bounds = svgDocument.Picture!.CullRect;
                    float scaleX = size / bounds.Width;
                    float scaleY = size / bounds.Height;
                    float scale = Math.Min(scaleX, scaleY);

                    canvas.Scale(scale);
                    canvas.DrawPicture(svgDocument.Picture);
                    canvas.Flush();

                    using (var image = SKImage.FromBitmap(bitmap))
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                        data.SaveTo(ret);
                }
                return ret;
            }
            catch (Exception ex)
            {

                throw new Exceptions.FavIconlibConvertException(ex.Message, ex);
            }
        }
    }
}

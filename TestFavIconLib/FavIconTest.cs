using System.Threading.Tasks;
using SkiaSharp;
using Svg.Skia;

namespace TestFavIconLib
{
    public class FavIconTest
    {
        [Fact]
        public async Task Test1()
        {
          var x = await FavIconlib.FavIcon.Test("https://computerbase.de");
           Assert.True(!string.IsNullOrEmpty(x));
        }

        [Fact]
        public async Task ConvertSvg()
        {
            using HttpClient client = new HttpClient();
            var req = await client.GetAsync("https://www.computerbase.de/favicon.svg");
            req.EnsureSuccessStatusCode();

            var datalod = await req.Content.ReadAsStreamAsync();
            var svgDocument = new SKSvg();
            svgDocument.Load(datalod);


            using (var bitmap = new SKBitmap((int)svgDocument.Picture!.CullRect.Width, (int)svgDocument.Picture.CullRect.Height))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.Transparent);
                canvas.DrawPicture(svgDocument.Picture);
                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Ico,100))
                using (var stream = File.OpenWrite("favicon.ico"))
                {
                    data.SaveTo(stream);
                }
            }

        }
    }
}

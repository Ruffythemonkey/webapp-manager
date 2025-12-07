using SkiaSharp;
using Svg.Skia;
using System.Text;
using System.Text.RegularExpressions;

namespace TestFavIconLib
{
    public class FavIconTest
    {
        [Fact]
        public async Task Test1()
        {
            var x = await FavIconlib.FavIcon.Test("https://computerbase.de", 64);

            using FileStream file = new FileStream("favicon.ico", FileMode.OpenOrCreate);
            x.Seek(0, SeekOrigin.Begin);
            x.CopyTo(file);
            file.Close();


           
        }

        [Fact]
        public async Task ConvertSvg()
        {
            int size = 128;
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:145.0) Gecko/20100101 Firefox/145.0");
            var req = await client.GetAsync("https://www.computerbase.de/favicon.svg");
            req.EnsureSuccessStatusCode();
            var datalod = await req.Content.ReadAsStringAsync();

            File.Delete("favicon.png");

            datalod = Regex.Replace(datalod, @"fill:color\(display-p3[^\)]*\);?", "");
            datalod = datalod.Replace(";;", ";");

            var svgDocument = new SKSvg();
            svgDocument.Load(new MemoryStream(Encoding.UTF8.GetBytes(datalod)));



            using (var bitmap = new SKBitmap(size, size))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.Transparent);

                var bounds = svgDocument.Picture.CullRect;
                float scaleX = size / bounds.Width;
                float scaleY = size / bounds.Height;
                float scale = Math.Min(scaleX, scaleY);

                canvas.Scale(scale);
                canvas.DrawPicture(svgDocument.Picture);
                canvas.Flush();


                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite("favicon.png"))
                {

                    data.SaveTo(stream);
                }
            }

        }
    }
}

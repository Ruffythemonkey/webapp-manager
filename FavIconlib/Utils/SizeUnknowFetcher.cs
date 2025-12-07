using FavIconlib.AbstractClasses;
using SkiaSharp;
using System.Reflection.Metadata;

namespace FavIconlib.Utils
{
    internal class SizeUnknowFetcher : Web
    {
        public static SizeUnknowFetcher Instance
        {
            get {
                if (field is null)
                {
                    field = new SizeUnknowFetcher();
                }
                return field;
            }
        }

        public async Task<List<Models.FavIcon>> SetUnknowSize(List<Models.FavIcon> favIcons)
        {

            foreach (var item in favIcons)
            {
                try
                {
                    if (item.type == "none" || item.sizes > 0 || item.IconUrl is null)
                    {
                        continue;
                    }


                    var req = await GetAsync(item.IconUrl.ToString());
                    req.EnsureSuccessStatusCode();
                    var data = await req.Content.ReadAsByteArrayAsync();
                    var codec = SKCodec.Create(new SKMemoryStream(data));
                    //int codecSize = 0;
                    //SKCodecFrameInfo? frame = null;

                    if (codec == null)
                        throw new Exceptions.FavIconlibConvertException("cannot read codec");

                    //foreach (var img in codec.FrameInfo)
                    //{
                    //    if (img.FrameRect.Width > codecSize)
                    //    {
                    //        frame = img;
                    //    }
                    //}


                    item.sizes = (uint)Math.Max(0, codec.Info.Width);
                    var opt = new SKImageInfo(codec.Info.Width, codec.Info.Height);
                    using SKBitmap pixels = SKBitmap.Decode(codec, opt);
                   
                    item.href = $"data:base64,{Convert.ToBase64String(ToPngBytes(pixels))}";
                }
                catch (Exception)
                {


                }

            }

            return favIcons;

        }


       private byte[] ToPngBytes(SKBitmap bmp)
        {
            using var image = SKImage.FromBitmap(bmp);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }


    }
}

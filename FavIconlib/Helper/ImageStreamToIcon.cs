using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace FavIconlib.Helper
{
    internal static class ImageStreamToIcon
    {
        public static Stream ConvertImageToIcon(Stream imageStream)
        {
            // MemoryStream für das Ergebnis
            var ret = new MemoryStream();

            // Load original
            using (var original = new Bitmap(imageStream))
            {
                // Konvertiere in 32bpp ARGB (alpha) — viele PNGs sind palettiert und verursachen Probleme
                using var bmp32 = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);
                using (var g = Graphics.FromImage(bmp32))
                {
                    g.Clear(Color.Transparent);
                    g.DrawImage(original, 0, 0, original.Width, original.Height);
                }

                // Save converted PNG in einen temporären MemoryStream
                using var pngStream = new MemoryStream();
                bmp32.Save(pngStream, ImageFormat.Png);
                pngStream.Position = 0;

                // BinaryWriter soll den underlying Stream nicht schließen
                using (var writer = new BinaryWriter(ret, Encoding.UTF8, leaveOpen: true))
                {
                    // ICONDIR (6 bytes)
                    writer.Write((ushort)0); // reserved
                    writer.Write((ushort)1); // type 1 = icon
                    writer.Write((ushort)1); // number of images

                    // ICONDIRENTRY (16 bytes)
                    int width = bmp32.Width >= 256 ? 0 : bmp32.Width;
                    int height = bmp32.Height >= 256 ? 0 : bmp32.Height;

                    writer.Write((byte)width);          // width
                    writer.Write((byte)height);         // height
                    writer.Write((byte)0);              // colors (0 if no palette)
                    writer.Write((byte)0);              // reserved

                    writer.Write((ushort)0);            // planes (0 for PNG in ICO)
                    writer.Write((ushort)0);            // bitcount (0 for PNG in ICO)

                    writer.Write((int)pngStream.Length);// size of data
                    writer.Write((int)(6 + 16));        // offset to data (ICONDIR + ICONDIRENTRY)

                    // write png bytes
                    writer.Write(pngStream.ToArray());
                    writer.Flush();
                }
            }

            ret.Position = 0;
            return ret;
        }

    }
}

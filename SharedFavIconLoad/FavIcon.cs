using System;
using System.Collections.Generic;
using System.Text;

namespace SharedFavIconLoad
{
    internal static class FavIcon
    {

        public static async Task<string> IconLoadAsync(Uri uri)
        {
            string iconname = uri.DnsSafeHost.ToLower();
            string file = App.AppLocalDataFolderPath("cache/" + iconname.Replace(".", "-") + ".ico");

            if (File.Exists(file))
            {
                return file;
            }

            var cli = httpClient.CreateClient();

            var request = await cli.GetAsync($"https://icons.duckduckgo.com/ip3/{iconname}.ico");
            request.EnsureSuccessStatusCode();

            if (!Directory.Exists(Path.GetDirectoryName(file)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file)!);
            }

            var content = await request.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync($"{file}.png", content);

            ImageToIconConverterService.ConvertToIcon($"{file}.png", file, 64);

            File.Delete($"{file}.png");

            return file;
        }
    }
}

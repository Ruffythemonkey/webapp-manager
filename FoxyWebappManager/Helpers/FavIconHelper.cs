namespace FoxyWebappManager.Helpers
{
    public static class FavIconHelper
    {
        public static async Task<string> IconLoadAsync(Uri uri)
        {
            var iconname = uri.DnsSafeHost.ToLower();
            var file = LocalAppDataPath.LocalDataFile("cache/" + iconname.Replace(".", "-") + ".ico");

            if (File.Exists(file))
            {
                return file;
            }

            using (var cli = new HttpClient())
            {
                var request = await cli.GetAsync($"https://icons.duckduckgo.com/ip3/{iconname}.ico");
                request.EnsureSuccessStatusCode();

                var content = await request.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync($"{file}.png", content);
            }

                ImageToIconConverterHelper
                    .ConvertToIcon($"{file}.png", file, 64);

            File.Delete($"{file}.png");

            return file;
        }
    }
}

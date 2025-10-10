using FoxyWebAppManager.Extensions;

namespace FoxyWebAppManager.Helpers
{
    public static class FavIconHelper
    {
        public static async Task<string> IconLoadAsync(Uri uri)
        {
            var iconname = $"{uri.Scheme}://{uri.DnsSafeHost}";

            var tempFolder = Path.GetTempPath();
            var tempfn = uri.GetHashCode().ToString().Replace("-", "");
            
            var file = Path.Combine(tempFolder,$"{tempfn}.ico");

            if (File.Exists(file))
            {
                return file;
            }

            using (var cli = new HttpClient())
            {
                var request = await cli.GetAsync($"https://www.google.com/s2/favicons?domain={iconname}&sz=64");
                if (!request.IsSuccessStatusCode && uri.DomainHasSubLevelDomain())
                {
                    request = await cli.GetAsync($"https://www.google.com/s2/favicons?domain={uri.GetTopLevelDomain()}&sz=64");
                }
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
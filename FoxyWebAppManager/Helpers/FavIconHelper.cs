using FoxyWebAppManager.Extensions;

namespace FoxyWebAppManager.Helpers
{
    public static class FavIconHelper
    {

        private static readonly HttpClient _client = new HttpClient();

        private static CancellationTokenSource _tokenSource = new CancellationTokenSource();

        private static readonly string _gstring = "https://www.google.com/s2/favicons?domain={0}&sz=64";

        private static readonly string _tmpfolder = Path.GetTempPath();

        public static async Task<string> IconLoadAsync(Uri uri)
        {
            await _tokenSource.CancelAsync();
            _tokenSource?.Dispose();
            _tokenSource = new CancellationTokenSource();
            try
            {
                return await IconLoadAsyncGoogle(uri);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string CreateFileName(Uri uri, out string filename)
        {
            var iconname = $"{uri.Scheme}://{uri.DnsSafeHost}";
            var tempfn = uri.GetHashCode().ToString().Replace("-", "");
            filename = Path.Combine(_tmpfolder, $"{tempfn}.ico");
            return iconname;
        }

        private async static void CreateIcon(this byte[] content, string file)
        {
            await File.WriteAllBytesAsync($"{file}.png", content);
            
            FavIconlib.Helper.ImageIconConverter
                .ConvertToIcon($"{file}.png", file, 64);
            File.Delete($"{file}.png");
        }

        private static async Task<string> IconLoadAsyncGoogle(Uri uri)
        {

            var iconname = CreateFileName(uri, out var file);
            
            if (File.Exists(file))
                return file;

            var request = await _client.GetAsync(string.Format(_gstring, iconname),
                _tokenSource.Token);

            if (!request.IsSuccessStatusCode && uri.DomainHasSubLevelDomain())
                request = await _client.GetAsync(string.Format(_gstring, uri.GetTopLevelDomain()),
                    _tokenSource.Token);

            if (!request.IsSuccessStatusCode)
            {
                return "";
            }

            (await request.Content.ReadAsByteArrayAsync())
                .CreateIcon(file);

            return file;
        }

        //private static async Task<string> IconLoadAsyncSelf(Uri uri)
        //{
        //    var iconname = CreateFileName(uri, out var filename);
            
        //    if (File.Exists(filename))
        //        return filename;

        //    var req = await _client.GetAsync(uri, _tokenSource.Token);

        //}
    }
}
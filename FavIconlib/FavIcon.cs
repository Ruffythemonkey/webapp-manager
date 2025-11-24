using FavIconlib.Utils;

namespace FavIconlib
{
    public static class FavIcon
    {
        private static readonly FavIconFetcher _fetcher = new();

        public static async Task<string> Test(string url)
            => await _fetcher.FavIconFetchSelf(url);
    }
}

using FavIconlib.Utils;

namespace FavIconlib
{
    public static class FavIcon
    {
        private static readonly FavIconFetcher _fetcher = new();

        public static async Task<Stream> Test(string url, int size)
            => await _fetcher.FavIconFetchSelf(url, size);
    }
}

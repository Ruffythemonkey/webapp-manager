using FavIconlib.Utils;

namespace FavIconlib.Extensions
{
    internal static class EnumarbaleFavIconsExtensions
    {
        extension(IEnumerable<Models.FavIcon> icons)
        {
            /// <summary>
            /// Sets the URL for all icon items in the collection.
            /// </summary>
            /// <param name="url">The URL to assign to each icon item. Cannot be null.</param>
            public List<Models.FavIcon> SetUrl(string url)
            {
                var ret = icons.ToList();
                foreach (var item in ret)
                    item.url = url;
                return ret;
            }

            public async Task<List<Models.FavIcon>> SetUnknowSizesData()
                => await SizeUnknowFetcher.Instance.SetUnknowSize(icons.ToList());
        }
    }

}

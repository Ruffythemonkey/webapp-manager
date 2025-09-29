namespace FoxyWebappManager.Extensions
{
    public static class UriExtension
    {
        public static string GetDomainNameWithoutExtension(this Uri uri)
        {
            return uri
                .DnsSafeHost
                .Split('.')
                .AsEnumerable()
                .Reverse()
                .Skip(1)
                .First();
        }

        public static string GetTopLevelDomain(this Uri uri)
        {
            var domainName = uri.GetDomainNameWithoutExtension();
            var root = uri.DnsSafeHost.Split(".").Last();
            return $"{uri.Scheme}://{domainName}.{root}";
        }

        public static bool DomainHasSubLevelDomain(this Uri uri)
            => uri.DnsSafeHost.Split(".").Count() > 1;
    }
}

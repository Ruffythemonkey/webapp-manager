namespace FoxyWebAppManager.Extensions
{
    public static class UriExtension
    {
        extension(Uri uri)
        {
            public string GetDomainNameWithoutExtension()
            {
                return uri
                    .DnsSafeHost
                    .Split('.')
                    .AsEnumerable()
                    .Reverse()
                    .Skip(1)
                    .First();
            }

            public string GetTopLevelDomain()
            {
                var domainName = uri.GetDomainNameWithoutExtension();
                var root = uri.DnsSafeHost.Split(".").Last();
                return $"{uri.Scheme}://{domainName}.{root}";
            }

            public bool DomainHasSubLevelDomain()
                => uri.DnsSafeHost.Split(".").Count() > 1;
        }
    }

}
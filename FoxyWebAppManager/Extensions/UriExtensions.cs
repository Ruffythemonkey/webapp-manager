namespace FoxyWebAppManager.Extensions
{
    public static class UriExtension
    {
        extension(Uri uri)
        {
            public string GetDomainNameWithoutExtension()
                => TLDExtractor.TLDExtractor.Extract(uri).Domain;

            public string GetTopLevelDomain()
                => $"{uri.Scheme}://{TLDExtractor.TLDExtractor.Extract(uri).EffectiveDomain}";

            public bool DomainHasSubLevelDomain()
                => uri.DnsSafeHost.Split(".").Count() > 1;
        }
    }

}
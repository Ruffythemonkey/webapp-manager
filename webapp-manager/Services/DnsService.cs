using System.Net;

namespace webapp_manager.Services
{
    public class DnsService
    {
        public async Task<bool> IsResolvableUrlAsync(string? url)
        {
            try
            {
                // Prüfen, ob die URL gültig ist
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || string.IsNullOrEmpty(uri.Host))
                    return false;

                // DNS-Abfrage für den Host durchführen
                var addresses = await Dns.GetHostAddressesAsync(uri.Host);
                return addresses.Length > 0;
            }
            catch
            {
                // Wenn ein Fehler auftritt, ist die URL ungültig
                return false;
            }

        }
        public bool IsResolvableUrl(string? url)
        {
            try
            {
                // Prüfen, ob die URL gültig ist
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || string.IsNullOrEmpty(uri.Host))
                    return false;

                // DNS-Abfrage für den Host durchführen
                var addresses = Dns.GetHostAddresses(uri.Host);
                return addresses.Length > 0;
            }
            catch
            {
                // Wenn ein Fehler auftritt, ist die URL ungültig
                return false;
            }

        }
    }
}

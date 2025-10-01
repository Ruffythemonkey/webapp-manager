using System.Threading.Tasks;
using FoxyWebappManager.Extensions;
using FoxyWebappManager.Helpers;
using FoxyWebappManager.Models;

namespace FoxyWebappManager.Services
{
    public class FireFoxEditService
    {
        public async Task CreateWebApp(FireFoxProfile profile, Uri url, string firfoxPath, string iconPath)
        {

            var rUrl = await UriCheckup(url);

            var setJson = profile
                .GetMainFolder()
                .SetJson(rUrl);

            var args = setJson.mainFolder.GetWindowsLnkArguments(rUrl);
            var oIcon = setJson.CopyIcon(iconPath, rUrl);

            var startmenu = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            var n = Path.Combine(startmenu, setJson.taskBarJson.GetShortcutRelativePath(rUrl));
            ShellShortcutApp.CreateShortcutWithAppId(n, firfoxPath, args, oIcon, setJson.taskBarJson.GetId(rUrl));
        }

        private async Task<Uri> UriCheckup(Uri url) 
        {
            try
            {
                using HttpClient client = new HttpClient();
                var req = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                req.EnsureSuccessStatusCode();
                return (req.RequestMessage!.RequestUri!);
            }
            catch (Exception)
            {

                throw;
            }
        
        }

    }
}

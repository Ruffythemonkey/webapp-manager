using System.Text.Json;
using FoxyWebappManager.Extensions;
using FoxyWebappManager.Models;

namespace FoxyWebappManager.Services
{
    public class FireFoxEditService
    {
        public void CreateWebApp(FireFoxProfile profile,Uri url, string firfoxPath, string iconPath)
        {
            var setJson = profile
                .GetMainFolder()
                .SetJson(url);
            var args = setJson.mainFolder.GetWindowsLnkArguments(url);
            var oIcon = setJson.CopyIcon(iconPath,url);

        
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var name = Path.Combine(desktop, url.GetDomainNameWithoutExtension()); 
            Helpers.IOHelper.CreateShortcut(name ,firfoxPath, args, oIcon);

            var startmenu = Environment.GetFolderPath (Environment.SpecialFolder.Programs);
            var n = Path.Combine(startmenu, setJson.taskBarJson.GetShortcutRelativePath(url));
            Helpers.IOHelper.CreateShortcut(n ,firfoxPath, args, oIcon);

        }

    }
}

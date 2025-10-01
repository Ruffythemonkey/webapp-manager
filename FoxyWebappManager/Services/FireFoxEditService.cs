using FoxyWebappManager.Extensions;
using FoxyWebappManager.Helpers;
using FoxyWebappManager.Models;

namespace FoxyWebappManager.Services
{
    public class FireFoxEditService
    {
        public void CreateWebApp(FireFoxProfile profile, Uri url, string firfoxPath, string iconPath)
        {
            var setJson = profile
                .GetMainFolder()
                .SetJson(url);
            var args = setJson.mainFolder.GetWindowsLnkArguments(url);
            var oIcon = setJson.CopyIcon(iconPath, url);

            var startmenu = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            var n = Path.Combine(startmenu, setJson.taskBarJson.GetShortcutRelativePath(url));
            ShellShortcutApp.CreateShortcutWithAppId(n, firfoxPath, args, oIcon, setJson.taskBarJson.GetId(url));
        }

    }
}

using FoxyWebAppManager.Models;
using static FoxyWebAppManager.Helpers.FireFoxIniParser;

namespace FoxyWebAppManager.Extensions
{
    public static class FireFoxProfileExtensions
    {
        extension(FireFoxProfile profile)
        {
            public FireFoxMainFolder GetMainFolder()
            {
                var _appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var _standarfolder = Path.Combine(_appdata, "Mozilla/Firefox");

                return new FireFoxMainFolder(
                    profile.IsRelative ? Path.Combine(_standarfolder, profile.Path) : profile.Path);
            }

            public async Task CreateWebApp(Uri url, string firfoxPath, string iconPath)
            {

                url = await url.UriMovedCheckup();

                var setJson = profile
                    .GetMainFolder()
                    .SetJson(url);

                iconPath = setJson.CopyIcon(iconPath);
                var args = setJson.GetWindowsLnkArguments(url);

                var startmenu = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                var destPath = Path.Combine(startmenu, setJson.taskbarTab.shortcutRelativePath);

                Shortcut.Shortcut.CreateShortcutWithAppId(destPath, firfoxPath, args, iconPath, setJson.taskbarTab.id);
            }
        }

   

    }
}

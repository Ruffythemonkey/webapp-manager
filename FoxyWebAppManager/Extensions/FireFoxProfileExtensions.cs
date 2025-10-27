using FoxyWebAppManager.Models;

namespace FoxyWebAppManager.Extensions
{
    public static class FireFoxProfileExtensions
    {
        extension(FireFoxProfile profile)
        {
            /// <summary>
            /// Retrieve all File/Dir Paths from Profile
            /// </summary>
            /// <returns></returns>
            public FireFoxMainFolder GetMainFolder()
            {
                var _appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var _standarfolder = Path.Combine(_appdata, "Mozilla/Firefox");

                return new FireFoxMainFolder(
                    profile.IsRelative ? Path.Combine(_standarfolder, profile.Path) : profile.Path);
            }

            /// <summary>
            /// Create Web-App in StartMenu Directory / FireFox Web-Apps
            /// </summary>
            /// <param name="url"></param>
            /// <param name="firfoxPath"></param>
            /// <param name="iconPath"></param>
            /// <returns></returns>
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

            /// <summary>
            /// cautions this delete all Profile Files
            /// </summary>
            public void RemoveProfileFolder()
            {
                var dir = profile.GetMainFolder().ProfilePath;

                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir,true);
                }
            }
        }
    }
}

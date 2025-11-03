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
                    Directory.Delete(dir, true);
                }
            }

            /// <summary>
            /// prof is profile in use by parent.lock file is locked
            /// </summary>
            /// <returns></returns>
            public bool IsProfileInUse()
            {
                var lockFile = profile.GetMainFolder().ParentLockFile;

                if (!File.Exists(lockFile))
                {
                    // Lock-Datei fehlt → Profil sehr wahrscheinlich frei
                    return false;
                }

                try
                {
                    // Versuche, die Datei exklusiv zu öffnen
                    // FileShare.None verhindert andere Prozesse auf diese Datei zuzugreifen
                    using var fs = new FileStream(lockFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    // Datei konnte geöffnet werden → Profil ist nicht in Benutzung
                    return false;
                }
                catch (IOException)
                {
                    // Datei gesperrt → Profil wird von Firefox verwendet
                    return true;
                }
                catch (UnauthorizedAccessException)
                {
                    // Zugriff verweigert → konservativ annehmen, dass Profil läuft
                    return true;
                }
                catch
                {
                    // Andere Fehler → sicherheitshalber Profil als aktiv markieren
                    return true;
                }
            }

            /// <summary>
            /// gives the folder of all Profiles
            /// </summary>
            /// <returns></returns>
            public string GetProfileMainDirectory()
                => new DirectoryInfo(profile.GetMainFolder().ProfilePath).Parent!.FullName;
        }

        extension(IEnumerable<FireFoxProfile> profiles)
        {

            /// <summary>
            /// prof is profiles in use by parent.lock files is locked
            /// </summary>
            /// <returns></returns>
            public Dictionary<FireFoxProfile, bool> IsProfilesInUse()
            {
                var ret = new Dictionary<FireFoxProfile, bool>();
                
                foreach (var item in profiles)
                    ret.TryAdd(item, item.IsProfileInUse());
                
                return ret;
            }
        }

    }
}

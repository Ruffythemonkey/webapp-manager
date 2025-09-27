using System.Text.Json;
using FoxyWebappManager.Models;
using Microsoft.Extensions.Hosting;

namespace FoxyWebappManager.Extensions
{
    public static class FireFoxProfielMainFolder
    {
        public static FireFoxMainFolder GetMainFolder(this FireFoxProfile profile)
        {
            var _appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var _standarfolder = Path.Combine(_appdata, "Mozilla/Firefox");

            return new FireFoxMainFolder(
                profile.IsRelative ? Path.Combine(_standarfolder, profile.Path) : profile.Path);
        }

        public static FireFoxTaskBarJson GetJson(this FireFoxMainFolder folder)
        {
            if (folder.IsTasbarTabsExist)
            {
                return JsonSerializer.Deserialize<FireFoxTaskBarJson>(File.ReadAllText(folder.JsonFile))!;
            }
            return new FireFoxTaskBarJson();
        }

        public static void CreateTaskbarFolder(this FireFoxMainFolder folder)
            => Directory.CreateDirectory(folder.TaskBarFolder);

        public static FireFoxMainFolder SetJson(this FireFoxMainFolder folder, Uri url)
        {
            folder.CreateTaskbarFolder();
            var json = folder.GetJson();
            
            var hostexist = json
               .taskbarTabs
               .Where(x => x.scopes.Any(s => s.hostname == url.DnsSafeHost))
               .FirstOrDefault();

            if (hostexist is null)
            {
                json.taskbarTabs.Add(new()
                {
                    startUrl = url.ToString(),
                    scopes = { new Scope() { hostname = url.DnsSafeHost } },
                    shortcutRelativePath = $@"Firefox Web-Apps\{url.GetDomainName()}.lnk",
                    userContextId = 0
                });
            }
            else { hostexist.startUrl = url.ToString(); }

            var jstring = JsonSerializer.Serialize(json);
            File.WriteAllText(folder.JsonFile, jstring);

            return folder;
        }

        //"C:\Program Files\Mozilla Firefox\firefox.exe" "-taskbar-tab" "157d7a5a-0b62-46ab-9f6d-1ec65c3e3994" "-new-window" "https://www.amazon.de" "-profile" "C:\Users\serap\AppData\Roaming\Mozilla\Firefox\Profiles\utei7iz0.Streaming" "-container" "0"

        public static string GetWindowsLnk(this FireFoxMainFolder folder,Uri uri)
        {
            var data = GetJson(folder)
                .taskbarTabs
                .Where(x => x.startUrl == uri.ToString())
                .FirstOrDefault();
            if (data is null)
            {
                throw new ArgumentNullException("url not found");
            }

            return $"\"-taskbar-tab\" \"{data.id}\" \"-new-window\" \"{uri.Scheme}://{uri.DnsSafeHost}\" \"-profile\" \"{folder.ProfielPath}\" \"-container\" \"0\" ";

        }

        public static string GetDomainName(this Uri uri)
        {
            return uri
                .DnsSafeHost
                .Split('.')
                .AsEnumerable()
                .Reverse()
                .Skip(1)
                .First();
        }

    }


}

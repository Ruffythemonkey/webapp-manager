using FoxyWebAppManager.Models;
using System.Text.Json;

namespace FoxyWebAppManager.Extensions
{
    public static class FireFoxMainFolderExtension
    {
        public static FireFoxMainFolder GetMainFolder(this FireFoxProfile profile)
        {
            var _appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var _standarfolder = Path.Combine(_appdata, "Mozilla/Firefox");

            return new FireFoxMainFolder(
                profile.IsRelative ? Path.Combine(_standarfolder, profile.Path) : profile.Path);
        }
       
        public static FireFoxTaskbarJson GetJson(this FireFoxMainFolder folder)
        {
            if (folder.IsTasbarTabsExist)
            {
                return JsonSerializer.Deserialize<FireFoxTaskbarJson>(File.ReadAllText(folder.JsonFile),FireFoxTaskbarJsonContext.Default.FireFoxTaskbarJson) 
                    ?? throw new FileNotFoundException();
            }
            return new FireFoxTaskbarJson();
        }

        public static void CreateTaskbarFolder(this FireFoxMainFolder folder)
            => Directory.CreateDirectory(folder.IconFolder); //Icon Folder ist ein SubDirectory und somit wird auch automatisch der Taskbarfolder kreiert

        public static (FireFoxMainFolder mainFolder, FireFoxTaskbarJson taskBarJson)
            SetJson(this FireFoxMainFolder folder, Uri url)
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
                    shortcutRelativePath = $@"Firefox Web-Apps\{url.GetDomainNameWithoutExtension()}.lnk",
                    userContextId = 0
                });
            }
            else { hostexist.startUrl = url.ToString(); }

            var jstring = JsonSerializer.Serialize(json,FireFoxTaskbarJsonContext.Default.FireFoxTaskbarJson);
            File.WriteAllText(folder.JsonFile, jstring);

            return (folder, json);
        }

        public static string GetWindowsLnkArguments(this FireFoxMainFolder folder, Uri uri)
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

        /// <summary>
        /// CopyIcon from Temp to FireFox relevat IconFolder with ID Name
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sourceIcon"></param>
        /// <param name="sourceUrl"></param>
        /// <returns>FireFox Icon Path</returns>
        public static string CopyIcon(this (FireFoxMainFolder mainFolder, FireFoxTaskbarJson taskBarJson) data, string sourceIcon, Uri sourceUrl)
        {
            var name = data
                .taskBarJson
                .taskbarTabs
                .First(x => x.startUrl == sourceUrl.ToString())
                .id;
            name = $"{name}.ico";
            name = Path.Combine(data.mainFolder.IconFolder, name);
            File.Copy(sourceIcon, name, true);
            return name;
        }


    }


}
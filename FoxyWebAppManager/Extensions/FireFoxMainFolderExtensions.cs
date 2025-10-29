using FoxyWebAppManager.Models;
using System.Text.Json;

namespace FoxyWebAppManager.Extensions
{
    public static class FireFoxMainFolderExtension
    {


        public static FireFoxTaskbarJson GetJson(this FireFoxMainFolder folder)
        {
            if (folder.IsTaskbarTabsJsonExist)
            {
                return JsonSerializer.Deserialize<FireFoxTaskbarJson>(File.ReadAllText(folder.JsonFile))
                    ?? throw new FormatException();
            }
            return new FireFoxTaskbarJson();
        }

        public static void CreateTaskbarFolder(this FireFoxMainFolder folder)
            => Directory.CreateDirectory(folder.IconFolder); //Icon Folder ist ein SubDirectory und somit wird auch automatisch der Taskbarfolder kreiert

        public static (FireFoxMainFolder mainFolder, TaskbarTab taskbarTab)
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
                hostexist = new TaskbarTab()
                {
                    startUrl = url.ToString(),
                    scopes = { new Scope() { hostname = url.DnsSafeHost } },
                    shortcutRelativePath = $@"Firefox Web-Apps\{url.GetDomainNameWithoutExtension()}.lnk",
                    userContextId = 0
                };
                json.taskbarTabs.Add(hostexist);
            }
            else { hostexist.startUrl = url.ToString(); }

            var jstring = JsonSerializer.Serialize(json);
            File.WriteAllText(folder.JsonFile, jstring);

            return (folder, hostexist);
        }

        public static string GetWindowsLnkArguments(this (FireFoxMainFolder folder, TaskbarTab taskbarTab) data, Uri uri)
           => $"\"-taskbar-tab\" \"{data.taskbarTab.id}\" \"-new-window\" \"{uri.Scheme}://{uri.DnsSafeHost}\" \"-profile\" \"{data.folder.ProfilePath}\" \"-container\" \"0\"";


        /// <summary>
        /// CopyIcon from Temp to FireFox relevat IconFolder with ID Name
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sourceIcon"></param>
        /// <param name="sourceUrl"></param>
        /// <returns>FireFox Icon Path</returns>
        public static string CopyIcon(this (FireFoxMainFolder mainFolder, TaskbarTab taskbarTab) data, string sourceIcon)
        {
            //check Icons and convert it when .png, .jpeg, .jpg
            sourceIcon = TryConvertIcon(sourceIcon);

            var destinationIcon = Path.Combine(data.mainFolder.IconFolder, $"{data.taskbarTab.id}.ico");

            if (File.Exists(sourceIcon))
                File.Move(sourceIcon, destinationIcon, true);

            return destinationIcon;
        }

        private static string TryConvertIcon(string sourceIcon)
        {
            var ext = Path.GetExtension(sourceIcon).ToLower();

            if (ext != ".ico")
            {
                var name = Path.GetFileNameWithoutExtension(sourceIcon);
                var tmpFullName = Path.Combine(Path.GetTempPath(), $"{name}.ico");
                var iconConvert = Helpers.ImageToIconConverterHelper.ConvertToIcon(sourceIcon, tmpFullName, 64);

                return iconConvert ? tmpFullName : throw new BadImageFormatException();

            }
            return sourceIcon;
        }
    }


}
using FoxyWebAppManager.Helpers;
using FoxyWebAppManager.Models;
using Microsoft.Windows.Storage.Pickers;
using System.Text.Json;

namespace FoxyWebAppManager.Extensions
{
    public static class TaskBarTabExtensions
    {
        extension(TaskbarTab taskbarTab)
        {
            public Uri GetStartUrl()
                => new(taskbarTab.startUrl);

            /// <summary>
            /// Get Domain Name eg https://google.com => google
            /// </summary>
            /// <returns></returns>
            public string DomainName()
                => taskbarTab.GetStartUrl().GetDomainNameWithoutExtension();

            /// <summary>
            /// Get Web-App Icon From Profile
            /// </summary>
            /// <param name="profile"></param>
            /// <returns></returns>
            public string GetIcon(FireFoxProfile profile)
                => Path.Combine(profile.GetMainFolder().IconFolder, $"{taskbarTab.id}.ico");

            /// <summary>
            /// retrive a new Icon
            /// </summary>
            /// <param name="profile"></param>
            /// <returns></returns>
            public async Task<string> SetIconAsync(FireFoxProfile profile)
            {
                var openPicker = new FileOpenPicker(App.MainWindow.AppWindow.Id);
                openPicker.FileTypeFilter.Add(".ico");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                var file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    return file.Path;
                }
                return taskbarTab.GetIcon(profile);
            }

            /// <summary>
            /// Update available Web-App
            /// </summary>
            public void UpdateWebApp(string Icon, FireFoxProfile profile)
            {
                var j = profile.GetMainFolder().GetJson();
                if (j is FireFoxTaskbarJson json)
                {
                    var t = json.taskbarTabs.First(x => x.id == taskbarTab.id);
                    t.startUrl = taskbarTab.startUrl;

                    var outputJsonString = JsonSerializer.Serialize(json);
                    File.WriteAllText(profile.GetMainFolder().JsonFile, outputJsonString);
                }

                if (Icon != taskbarTab.GetIcon(profile))
                {
                    Helpers.ImageToIconConverterHelper.ConvertToIcon(Icon, taskbarTab.GetIcon(profile), 64);
                }

            }

            public void RemoveWebAppIO(FireFoxProfile profile)
            {
                var env_startmenu = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                var lnkPath = Path.Combine(env_startmenu, taskbarTab.shortcutRelativePath);
                
                Psi.UnpinTaskbar(lnkPath);

                if (File.Exists(lnkPath))
                    File.Delete(lnkPath);

                if (File.Exists(taskbarTab.GetIcon(profile)))
                    File.Delete(taskbarTab.GetIcon(profile));

                var json = profile.GetMainFolder().GetJson();
                json.taskbarTabs.Remove(taskbarTab);
                json.WriteJson(profile);

            }
        }

        public static bool IsAnyTaskBarTabItemInStartMenu(this List<TaskbarTab> tabs)
        {
            var startMenu = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            foreach (var item in tabs)
            {
                var p = Path.Combine(startMenu, item.shortcutRelativePath);
                if (File.Exists(p))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

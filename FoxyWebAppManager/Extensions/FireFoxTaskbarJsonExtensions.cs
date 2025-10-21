using FoxyWebAppManager.Models;

namespace FoxyWebAppManager.Extensions
{
    public static class FireFoxTaskbarJsonExtensions
    {

        extension(FireFoxTaskbarJson json)
        {
            public TaskbarTab GetTaskBarTabItem(Uri sourceUri)
                => json.taskbarTabs.First(x => x.startUrl == sourceUri.ToString());

            public string GetId(Uri sourceUri)
                => json.GetTaskBarTabItem(sourceUri).id;

            public string GetShortcutRelativePath(Uri sourceUri)
                => json.GetTaskBarTabItem(sourceUri).shortcutRelativePath;

            /// <summary>
            /// Return a list of Web-Apps this available in StartMenu & Registert in Firefox
            /// </summary>
            /// <returns></returns>
            public List<TaskbarTab> GetAvailableWebApps()
            {
                var Startmenu = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                return json.taskbarTabs.Where(x =>
                {
                    var webApp = Path.Combine(Startmenu, x.shortcutRelativePath);
                    var exist = File.Exists(webApp);
                    return exist;

                }).ToList();
            }
        }
    }
}
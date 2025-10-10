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
        }
    }
}
using FoxyWebappManager.Models;

namespace FoxyWebappManager.Extensions
{
    public static class FireFoxTaskBarJsonExtension
    {

        public static TaskbarTab GetTaskBarTabItem(this FireFoxTaskBarJson json, Uri sourceUri)
            => json.taskbarTabs.First(x => x.startUrl == sourceUri.ToString());

        public static string GetId(this FireFoxTaskBarJson json,Uri sourceUri)
            => json.GetTaskBarTabItem(sourceUri).id;

        public static string GetShortcutRelativePath(this FireFoxTaskBarJson json, Uri sourceUri)
            => json.GetTaskBarTabItem(sourceUri).shortcutRelativePath;

    }
}

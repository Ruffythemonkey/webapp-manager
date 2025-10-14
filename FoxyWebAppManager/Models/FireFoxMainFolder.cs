namespace FoxyWebAppManager.Models
{
    public class FireFoxMainFolder
    {
        public string ProfielPath { get; }

        public string JsonFile
        {
            get => Path.Combine(ProfielPath, "taskbartabs", "taskbartabs.json");
        }

        public string TaskBarFolder
        {
            get => Path.Combine(ProfielPath, "taskbartabs");
        }

        public bool IsTaskbarTabsJsonExist
        {
            get => File.Exists(JsonFile);
        }

        public string IconFolder
        {
            get => Path.Combine(TaskBarFolder, "icons");
        }

        public string StartMenuLnkFolder
        {
            get {
                var s = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                return Path.Combine(s, "Firefox Web-Apps");
            }
        }

        public FireFoxMainFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(path);
            }
            ProfielPath = path;
        }

    }
}
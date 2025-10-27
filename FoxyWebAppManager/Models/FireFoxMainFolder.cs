namespace FoxyWebAppManager.Models
{
    public class FireFoxMainFolder
    {
        /// <summary>
        /// Profile Directory 
        /// </summary>
        public string ProfilePath { get; }

        /// <summary>
        /// Path taskbartabs.json
        /// </summary>
        public string JsonFile
        {
            get => Path.Combine(ProfilePath, "taskbartabs", "taskbartabs.json");
        }

        public string TaskBarFolder
        {
            get => Path.Combine(ProfilePath, "taskbartabs");
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
            ProfilePath = path;
        }

    }
}
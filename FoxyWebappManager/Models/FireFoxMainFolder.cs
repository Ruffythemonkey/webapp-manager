

using Windows.Web.Syndication;

namespace FoxyWebappManager.Models
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

        public bool IsTasbarTabsExist
        {
            get => File.Exists(JsonFile);
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

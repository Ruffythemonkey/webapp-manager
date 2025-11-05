
namespace FoxyWebAppManager.Models
{
    public class FireFoxProfile
    {
        public string Section { get; set; } = "";
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public bool IsRelative { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is FireFoxProfile profile &&
                   Name == profile.Name &&
                   Path == profile.Path;
        }

        public override int GetHashCode() => HashCode.Combine(Name, Path);
    }
}
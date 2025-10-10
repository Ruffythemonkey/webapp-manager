namespace FoxyWebAppManager.Models
{
    public class FireFoxProfile
    {
        public string Section { get; set; } = "";
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public bool IsRelative { get; set; }
    }
}
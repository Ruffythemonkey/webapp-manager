using System.Text.Json.Serialization;

namespace FoxyWebAppManager.Models
{

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(FireFoxTaskbarJson))]
    internal partial class FireFoxTaskbarJsonContext : JsonSerializerContext
    {
    }

    public class FireFoxTaskbarJson
    {
        public int version { get; set; } = 1;

        /// <summary>
        /// Representet Web-Apps
        /// </summary>
        public List<TaskbarTab> taskbarTabs { get; set; } = [];
    }

    public class Scope
    {
        public string hostname { get; set; } = null!;
    }

    public class TaskbarTab
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public List<Scope> scopes { get; set; } = [];
        public int userContextId { get; set; }
        public string startUrl { get; set; } = null!;
        public string shortcutRelativePath { get; set; } = null!;
    }
}
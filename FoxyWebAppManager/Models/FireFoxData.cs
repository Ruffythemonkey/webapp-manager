using System.Text.Json.Serialization;

namespace FoxyWebAppManager.Models
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(FireFoxData))]
    internal partial class FireFoxDataJsonContext : JsonSerializerContext
    {
    }
    public class FireFoxData
    {
        public string Path { get; set; } = string.Empty;
    }
}
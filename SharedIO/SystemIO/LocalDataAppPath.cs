using System.Reflection;
using System.Text.Json;

namespace System.IO;

internal static class LocalDataAppPath
{
    public static string LocalDataFile(string file = "")
    {
        var _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appname = Path.Combine(
            _localApplicationData,
            $"{Assembly.GetEntryAssembly()?.GetName().Name}\\ApplicationData",
            file
            .Replace("/", "\\"));

        var dirpath = Path.GetDirectoryName(appname)!;

        if (!Directory.Exists(dirpath))
        {
            Directory.CreateDirectory(dirpath);
        }

        return appname;
    }

    public static string LocalSettingsPath { get => LocalDataFile("appsettings.json"); } 

    public static void Save<T>(T data)
    {
        var str = JsonSerializer.Serialize(data);
        File.WriteAllText(LocalSettingsPath, str );
    }

    public static T? Read<T>()
    {
        if (!File.Exists(LocalSettingsPath))
            return default;

        var str = File.ReadAllText(LocalSettingsPath);
        return JsonSerializer.Deserialize<T>(str);
    }

}
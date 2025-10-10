using System.Reflection;

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
}
using FoxyWebAppManager.Models;
using System.Text.Json;

namespace FoxyWebAppManager.Extensions
{
    public static class FireFoxPathExtensions
    {

        private static readonly string dataPath = LocalDataAppPath.LocalDataFile("ffpath.json");

        public static FireFoxData GetSavedFireFoxData()
        {
            var programm = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var defFireFox = Path.Combine(programm, @"Mozilla Firefox\Firefox.exe");

            if (File.Exists(dataPath))
            {
                var fr = File.ReadAllText(dataPath);
                return JsonSerializer.Deserialize<FireFoxData>(fr, FireFoxDataJsonContext.Default.FireFoxData)!;
            }
            else if (File.Exists(defFireFox))
            {
                return new FireFoxData() { Path = defFireFox };
            }
            return new FireFoxData();
        }

        public static void Save(this FireFoxData data)
        {
            var s = JsonSerializer.Serialize(data, typeof(FireFoxData), FireFoxDataJsonContext.Default);
            File.WriteAllText(dataPath, s);
        }
    }
}
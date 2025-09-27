using System.Text.Json;
using FoxyWebappManager.Models;

namespace FoxyWebappManager.Extensions
{
    public static class FireFoxPathExtension
    {

        private static readonly string dataPath = LocalAppDataPath.LocalDataFile("ffpath.json");

        public static FireFoxData GetSavedFireFoxData() 
        {
            var programm = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var defFireFox = Path.Combine(programm, @"Mozilla Firefox\Firefox.exe");

            if (File.Exists(dataPath))
            {
                var fr = File.ReadAllText(dataPath);
                return JsonSerializer.Deserialize<FireFoxData>(fr)!;
            }
            else if (File.Exists(defFireFox))
            {
                return new FireFoxData() { Path = defFireFox };
            }
            return new FireFoxData();
        }

        public static void Save(this FireFoxData data) 
        {
            var s = JsonSerializer.Serialize(data);
            File.WriteAllText(dataPath, s);
        }
    }
}

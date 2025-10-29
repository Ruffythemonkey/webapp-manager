using FoxyWebAppManager.Models;
using System.Diagnostics;
using System.Text.Json;

namespace FoxyWebAppManager.Extensions
{
    public static class FireFoxDataExtensions
    {
        private static readonly string dataPath = LocalDataAppPath.LocalDataFile("ffpath.json");


        extension(FireFoxData foxData)
        {

            public void CreateProfile(string name)
            {
                using var p = new Process();
                p.StartInfo.FileName = foxData.Path;
                p.StartInfo.Arguments = $"-CreateProfile {name}";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
            }

            public void Save()
            {
                var s = JsonSerializer.Serialize(foxData);
                File.WriteAllText(dataPath, s);
            }
        }

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

    }
}

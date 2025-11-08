using FoxyWebAppManager.Models;
using System.Diagnostics;
using System.Text.Json;

namespace FoxyWebAppManager.Extensions
{
    public static class FireFoxDataExtensions
    {
        extension(FireFoxData foxData)
        {
            public async Task CreateProfile(string name)
            {
                using var p = new Process();
                p.StartInfo.FileName = foxData.Path;
                p.StartInfo.Arguments = $"-CreateProfile {name}";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                await p.WaitForExitAsync();
            }

            public FireFoxData GetSavedFireFoxData()
            {
                var programm = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                var defFireFox = Path.Combine(programm, @"Mozilla Firefox\Firefox.exe");


                if (File.Exists(App.Settings.FireFoxApp.Path))
                {
                    return App.Settings.FireFoxApp;
                }
                else if (File.Exists(defFireFox))
                {
                    App.Settings.FireFoxApp.Path = defFireFox;
                }
                return App.Settings.FireFoxApp;
            }
        }


    }
}

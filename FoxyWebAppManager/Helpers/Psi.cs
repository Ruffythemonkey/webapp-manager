using System.Diagnostics;

namespace FoxyWebAppManager.Helpers
{
    internal static class Psi
    {
        public static void UnpinTaskbar(string appPath)
        {
            var psi = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"(New-Object -ComObject Shell.Application).Namespace((Split-Path '{appPath}')).ParseName((Split-Path '{appPath}' -Leaf)).InvokeVerb('taskbarunpin')",
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using Process proc = new Process();
            proc.StartInfo = psi;
            proc.Start();
            proc.WaitForExit();
        }
    }
}

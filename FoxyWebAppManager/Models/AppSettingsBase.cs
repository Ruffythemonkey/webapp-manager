using CommunityToolkit.Mvvm.ComponentModel;
using FoxyWebAppManager.Extensions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FoxyWebAppManager.Models
{
    
    public abstract class AppSettingsBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void SetProperty<T>(ref T field, T value, Action? onChanged, [CallerMemberName] string prop = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                onChanged?.Invoke();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }

        /// <summary>
        /// Checks the default FireFox Path is exist
        /// </summary>
        /// <returns></returns>
        public static string CheckDefaultFireFoxPath()
        {
            var programm = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var defFireFox = System.IO.Path.Combine(programm, @"Mozilla Firefox\firefox.exe");
            return File.Exists(defFireFox) ? new FileInfo(defFireFox).FullName : string.Empty;
        }


    }
}

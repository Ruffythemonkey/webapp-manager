using FoxyWebAppManager.Extensions;

namespace FoxyWebAppManager.Models
{
    public class FireFoxData : AppSettingsBase
    {
        public string Path
        {
            get;
            set => SetProperty(ref field, value, App.Settings.Save);
        } = CheckDefaultFireFoxPath();

    }
}
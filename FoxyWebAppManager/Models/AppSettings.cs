using FoxyWebAppManager.Extensions;
using Microsoft.UI.Xaml;

namespace FoxyWebAppManager.Models
{
    public class AppSettings : AppSettingsBase
    {
 
        public ElementTheme ElementTheme 
        { 
            get;
            set => SetProperty(ref field, value, this.Save);
        } = ElementTheme.Default;

        public FireFoxData FireFoxApp { get; set; } = new();
    }
}

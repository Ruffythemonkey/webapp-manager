using FoxyWebAppManager.Extensions;
using Microsoft.UI.Xaml;

namespace FoxyWebAppManager.Models
{
    public class AppSettings
    {
        public ElementTheme ElementTheme 
        { 
            get;
            set => SetProperty(ref field, value, this.Save);
        } = ElementTheme.Default;


        private void SetProperty<T>(ref T field, T value, Action onChanged)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                onChanged.Invoke();
            }
        }

    }
}

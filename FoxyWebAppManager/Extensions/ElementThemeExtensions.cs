using Microsoft.UI;
using Microsoft.UI.Xaml;
using Windows.UI.ViewManagement;

namespace FoxyWebAppManager.Extensions;

public static class ElementThemeExtensions
{
    extension(ElementTheme theme)
    {
        /// <summary>
        /// Toggle Theme
        /// </summary>
        /// <returns></returns>
        public ElementTheme ToggleTheme()
            => theme.CheckDefaultTheme() == ElementTheme.Light
                ? ElementTheme.Dark : ElementTheme.Light;

        /// <summary>
        /// Check the Theme and gives by default the right value eg light, dark
        /// </summary>
        /// <returns></returns>
        public ElementTheme CheckDefaultTheme()
        {
            if (theme == ElementTheme.Default)
            {
                var uiSettings = new UISettings();
                var background = uiSettings.GetColorValue(UIColorType.Background);
               
                return background == Colors.White ? ElementTheme.Light : ElementTheme.Dark;
            }
            return theme;
        }

        /// <summary>
        /// Get Application Theme from ElementTheme for App Settings
        /// </summary>
        /// <returns></returns>
        public ApplicationTheme GetApplicationThemeFromElementTheme()
        {
            return theme.CheckDefaultTheme() switch
            {
                ElementTheme.Light => ApplicationTheme.Light,
                ElementTheme.Dark => ApplicationTheme.Dark,
                _ => ApplicationTheme.Light,
            };
        }

    }


}

using Microsoft.UI;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace FoxyWebAppManager.Extensions
{
   public static class ElementThemeExtensions
    {
        extension(ElementTheme theme) 
        {
            public ElementTheme ToggleTheme()
            {
                if (theme == ElementTheme.Default)
                {
                    var uiSettings = new UISettings();
                    var background = uiSettings.GetColorValue(UIColorType.Background);

                    return background == Colors.White ? ElementTheme.Dark : ElementTheme.Light;
                }

                return theme == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light;

            }

        }
    }
}

using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FoxyWebAppManager.ContentDialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DialogException : Page
    {
        public DialogException()
        {
            InitializeComponent();
        }

        public string Message { get; set; } = string.Empty;

        public ContentDialog ContentDialog
        {
            get {
                if (field == null)
                {
                    field = new ContentDialog()
                    {
                        PrimaryButtonText = "OK",
                        XamlRoot = App.MainWindow.Content.XamlRoot,
                        RequestedTheme = App.Settings.ElementTheme,
                        Content = this
                    };
                }
                return field;
            }
        }

    }

}

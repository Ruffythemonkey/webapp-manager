using CommunityToolkit.WinUI;
using FoxyWebAppManager.Extensions;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FoxyWebAppManager.ContentDialogs;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DialogRemoveProfile : Page
{
    public DialogRemoveProfile()
    {
        InitializeComponent();
    }

    public ContentDialog ContentDialog
    {
        get {
            if (field == null)
            {
                field = new ContentDialog()
                {
                    PrimaryButtonText = "ProfileRemoveYes".GetLocalized(),
                    SecondaryButtonText = "Cancel".GetLocalized(),
                    XamlRoot = App.MainWindow.Content.XamlRoot,
                    RequestedTheme = App.Settings.ElementTheme,
                    Content = this
                };
            }
            return field;
        }
    }
}

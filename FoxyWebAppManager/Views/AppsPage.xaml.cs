using FoxyWebAppManager.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace FoxyWebAppManager.Views;

public sealed partial class AppsPage : Page
{
    public AppsViewModel ViewModel
    {
        get;
    }

    public AppsPage()
    {
        ViewModel = App.GetService<AppsViewModel>();
        InitializeComponent();
    }
}

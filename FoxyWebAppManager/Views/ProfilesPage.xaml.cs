using FoxyWebAppManager.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace FoxyWebAppManager.Views;

public sealed partial class ProfilesPage : Page
{
    public ProfilesViewModel ViewModel
    {
        get;
    }

    public ProfilesPage()
    {
        ViewModel = App.GetService<ProfilesViewModel>();
        InitializeComponent();
    }
}

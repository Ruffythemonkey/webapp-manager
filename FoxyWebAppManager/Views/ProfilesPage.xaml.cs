using CommunityToolkit.WinUI;
using FoxyWebAppManager.ContentDialogs;
using FoxyWebAppManager.Extensions;
using FoxyWebAppManager.Models;
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

using CommunityToolkit.Mvvm.ComponentModel;
using FoxyWebappManager.Contracts.Services;
using Microsoft.UI.Xaml.Navigation;

namespace FoxyWebappManager.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private partial bool IsBackEnabled { get; set; }

    [ObservableProperty]
    private partial object? Selected { get; set; }

    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;
        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using FoxyWebAppManager.Contracts.Services;
using FoxyWebAppManager.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

namespace FoxyWebAppManager.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    public partial bool IsBackEnabled { get; set; }

    [ObservableProperty]
    public partial object? Selected { get; set; }

    public List<string> Themes { get; set; } = Enum.GetNames(typeof(ElementTheme)).ToList();

    [ObservableProperty]
    public partial string SelectedTheme { get; set; } = AppSettingsExtensions.GetSettings.ElementTheme.ToString();

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

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

    partial void OnSelectedThemeChanged(string value)
    {
        if (!string.IsNullOrEmpty(value) && App.MainWindow.Content is FrameworkElement element)
        {
            element.RequestedTheme = value switch
            {
                "Light" => ElementTheme.Light,
                "Dark" => ElementTheme.Dark,
                "Default" => ElementTheme.Default,
                _ => ElementTheme.Default
            };
            Helpers.TitleBarHelper.UpdateTitleBar(element.RequestedTheme);
            AppSettingsExtensions.GetSettings.ElementTheme = element.RequestedTheme;
        }
    }

}

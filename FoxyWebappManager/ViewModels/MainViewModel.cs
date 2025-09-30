using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoxyWebappManager.Contracts.ViewModels;
using FoxyWebappManager.Helpers;
using FoxyWebappManager.Models;
using FoxyWebappManager.Services;
using Microsoft.UI.Dispatching;
using Windows.Storage.Pickers;
using static FoxyWebappManager.Helpers.FireFoxIniParser;


namespace FoxyWebappManager.ViewModels;

public partial class MainViewModel(FireFoxEditService fireFoxEditService) : ObservableRecipient, INavigationAware
{
    private DispatcherQueue _dispatcher = DispatcherQueue.GetForCurrentThread();
    private bool IsValidHost { get; set; }

    private bool CanSaveExecute => IsValidHost && File.Exists(FireFoxPath);

    [ObservableProperty]
    public partial List<FireFoxProfile> Profiels { get; set; }

    [ObservableProperty]
    public partial string Hostname { get; set; }

    [ObservableProperty]
    public partial string FavIcon { get; set; } = Path.Combine(AppContext.BaseDirectory, "/Assets/WindowIcon.ico");

    [ObservableProperty]
    public partial FireFoxProfile SelectedProfiel { get; set; }

    [ObservableProperty]
    public partial string FireFoxPath { get; set; } = Extensions.FireFoxPathExtension.GetSavedFireFoxData().Path;

    [ObservableProperty]
    public partial bool IsCustomizeUserStyle { get; set; }

    partial void OnHostnameChanged(string value)
    {
        _ = GetFavIcon(value);
    }

    partial void OnFireFoxPathChanged(string value)
    {
        if (File.Exists(value))
        {
            Extensions.FireFoxPathExtension.Save(new FireFoxData() { Path = value });
        }
    }

    partial void OnProfielsChanged(List<FireFoxProfile> value)
    {
        if (value.Any())
        {
            SelectedProfiel = Profiels.First();
            
        }
    }

    partial void OnSelectedProfielChanged(FireFoxProfile value)
    {
       IsCustomizeUserStyle = new FireFoxCssHelper(SelectedProfiel).IsUserChromeActive;
    }
    
    private async Task GetFavIcon(string v)
    {
        try
        {
            if (v.IsUrl() && await v.ToString().IsResolvableUrlAsync())
            {
                _dispatcher.TryEnqueue(() =>
                {
                    IsValidHost = true;
                });
                FavIcon = await FavIconHelper.IconLoadAsync(new Uri(v));
            }
            else
            {
                _dispatcher.TryEnqueue(() =>
                {
                    FavIcon = DefaultFavIcon;
                    IsValidHost = false;
                });
            }
        }
        catch (Exception)
        {
            _dispatcher.TryEnqueue(() =>
            {
                FavIcon = DefaultFavIcon;
            });
        }
        finally
        {
            _dispatcher.TryEnqueue(() =>
            {
                SaveCommand.NotifyCanExecuteChanged();
            });
        }
    }

    [RelayCommand(CanExecute = nameof(CanSaveExecute))]
    private void Save() =>
        fireFoxEditService.CreateWebApp(SelectedProfiel, new Uri(Hostname), FireFoxPath, FavIcon);

    [RelayCommand]
    private async Task OpenDialogFireFox()
    {
        var openPicker = new FileOpenPicker();
        var window = App.MainWindow;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.FileTypeFilter.Add(".exe");
        var file = await openPicker.PickSingleFileAsync();
        if (file != null)
        {
            FireFoxPath = file.Path;
            Extensions.FireFoxPathExtension.Save(new() { Path = file.Path });
        }
    }

    [RelayCommand]
    private void SwitchUserChrome(bool activate) =>
        new FireFoxCssHelper(SelectedProfiel).ActivateUserChrome(activate);

    private string DefaultFavIcon
    {
        get => Path.Combine(AppContext.BaseDirectory, "/Assets/WindowIcon.ico");
    }

    public void OnNavigatedTo(object parameter)
    {
        Profiels = IniReaderFireFox
        .LoadProfilesFromInstalledFF();
    }

    public void OnNavigatedFrom() { }
}

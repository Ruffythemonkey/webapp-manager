using System.ComponentModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using webapp_manager.Contracts.Services;
using webapp_manager.Services;
using Windows.Storage;
using Windows.Storage.Pickers;


namespace webapp_manager.ViewModels;

public partial class MainViewModel(FaviconWebLoadService favicon, ILocalSettingsService settings, DnsService dns) : ObservableRecipient
{

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        CreateCommand.NotifyCanExecuteChanged();
    }

    private DispatcherQueue dispatcher = DispatcherQueue.GetForCurrentThread();

    [ObservableProperty]
    public partial string? Webappname { get; set; }

    [ObservableProperty]
    public partial Uri? Icon { get; set; }

    [ObservableProperty]
    public partial string? Webappurl { get; set; }

    [ObservableProperty]
    public partial string? Firefoxpath { get; set; }

    [ObservableProperty]
    public partial string? Message { get; set; }

    [ObservableProperty]
    public partial bool IsBussy { get; set; }

    [RelayCommand]
    private void CloseMessage() => Message = null;

    [RelayCommand(CanExecute = nameof(CreateCanExecute))]
    private void Create()
    {
        string local = App.AppLocalDataFolderPath($"profiles\\{Webappname}");
        string profiledir = $"\"{local}\\firefox\\profile\"";
        string shortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{Webappname}.lnk");

        Debug.WriteLine(shortcut);

        if (!Directory.Exists(local))
        {
            Directory.CreateDirectory(local);
            IOServices.IOServices.CopyFolder("Local", local);
        }

        string output = $"{Firefoxpath} --profile {profiledir} --no-remote --url {Webappurl}";
        Debug.WriteLine(output);
        IOServices.IOServices.CreateShortcut(
            shortcut, 
            Firefoxpath!, 
            $"--profile {profiledir} --no-remote --url {Webappurl}",
            Icon?.ToString());

       // IOServices.CreateShortcut($"")
        //File.CreateSymbolicLink($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}{Webappname}.lnk", output);
    }

    private bool CreateCanExecute()
    {
        return !string.IsNullOrEmpty(Webappname) &&
               !string.IsNullOrEmpty(Firefoxpath) &&
               Uri.TryCreate(Icon?.ToString(), UriKind.Absolute, out _) &&
               dns.IsResolvableUrl(Webappurl);
    }


    [RelayCommand]
    private async Task IconAsync()
    {

        if (!await dns.IsResolvableUrlAsync(Webappurl))
        {
            dispatcher.TryEnqueue(() => { Message = "Url cannot resolve"; });
            return;
        }

        try
        {
            dispatcher.TryEnqueue(() => { IsBussy = true; });
            var content = await favicon.IconLoadAsync(new Uri(Webappurl!));
            dispatcher.TryEnqueue(() => { Icon = new(content); });

        }
        catch (Exception ex)
        {
            dispatcher.TryEnqueue(() => { Message = ex.Message; });
        }
        finally
        {
            dispatcher.TryEnqueue(() => { IsBussy = false; });
        }
    }

    [RelayCommand]
    private async Task LocalIconAsync()
    {
        FileOpenPicker picker = new();
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        picker.ViewMode = PickerViewMode.Thumbnail;
        picker.FileTypeFilter.Add(".ico");

        StorageFile file = await picker.PickSingleFileAsync();
        if (file != null)
            dispatcher.TryEnqueue(() => { Icon = new(file.Path); });
    }

    [RelayCommand]
    private async Task ExploreFirefox()
    {
        IsBussy = true;
        var picker = new FileOpenPicker();

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

        picker.ViewMode = PickerViewMode.Thumbnail;
        picker.FileTypeFilter.Add(".exe");

        StorageFile file = await picker.PickSingleFileAsync();

        if (file != null && file.DisplayName == "firefox")
        {
            Firefoxpath = file.Path;
        }
        else if (file != null)
        {
            var dialog = new ContentDialog
            {
                Title = "Failure",
                Content = "Please select firefox.exe",
                CloseButtonText = "OK",
                XamlRoot = App.MainWindow.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }
        IsBussy = false;
    }

    public static bool InvertBool(bool b) => !b;

}

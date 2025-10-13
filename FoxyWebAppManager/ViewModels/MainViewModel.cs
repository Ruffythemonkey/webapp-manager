using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoxyWebAppManager.Models;
using FoxyWebAppManager.Extensions;
using static FoxyWebAppManager.Helpers.FireFoxIniParser;


namespace FoxyWebAppManager.ViewModels;

public partial class MainViewModel : BaseViewModel
{

    private bool CanWebAppSaveExecute => IsValidHost && File.Exists(FireFoxData.Path);

    [ObservableProperty]
    public partial bool IsValidHost { get; set; }

    [ObservableProperty]
    public partial List<FireFoxProfile> FoxProfiles { get; set; }

    [ObservableProperty]
    public partial FireFoxProfile SelectedFireFoxProfile { get; set; }

    [ObservableProperty]
    public partial string FavIcon { get; set; } = "/Assets/WindowIcon.ico";

    [ObservableProperty]
    public partial FireFoxData FireFoxData { get; set; } = FireFoxPathExtensions.GetSavedFireFoxData();

    [ObservableProperty]
    public partial string WebHost { get; set; }

    [RelayCommand]
    private async Task OpenFireFoxPath() => await this.OpenFireFoxPathEx();

    [RelayCommand]
    private async Task OpenFavIconFromPath() => await this.OpenIconPathEx();

    [RelayCommand(CanExecute = nameof(CanWebAppSaveExecute))]
    private void SaveWebApp()
    {

    }

    partial void OnWebHostChanged(string value)
      => _ = this.ChangeFavIconByWebHostChanged();

    public override void OnNavigatedFrom()
    {
        //throw new NotImplementedException();
    }

    public override void OnNavigatedTo(object parameter)
        => FoxProfiles = IniReaderFireFox.LoadProfilesFromInstalledFF();

    partial void OnFoxProfilesChanged(List<FireFoxProfile> value)
    {
        if (value.Count > 0)
        {
            SelectedFireFoxProfile = value.First();
        }
    }
}

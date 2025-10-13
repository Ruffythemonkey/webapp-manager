using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoxyWebAppManager.Models;
using FoxyWebAppManager.Extensions;
using System.Collections.ObjectModel;
using static FoxyWebAppManager.Helpers.FireFoxIniParser;


namespace FoxyWebAppManager.ViewModels;

public partial class MainViewModel : BaseViewModel
{

    [ObservableProperty]
    public partial List<FireFoxProfile> FoxProfiles { get; set; }

    [ObservableProperty]
    public partial FireFoxProfile SelectedFireFoxProfile { get; set; }

    [ObservableProperty]
    public partial string FavIcon { get; set; } = "/Assets/WindowIcon.ico";

    [ObservableProperty]
    public partial FireFoxData FireFoxData { get; set; } = FireFoxPathExtensions.GetSavedFireFoxData();


    [RelayCommand]
    private async Task OpenFireFoxPath() => await this.OpenFireFoxPathEx();

    [RelayCommand]
    private async Task OpenFavIconFromPath() => await this.OpenIconPathEx();

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

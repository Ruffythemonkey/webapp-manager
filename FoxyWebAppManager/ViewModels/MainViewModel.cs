using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoxyWebAppManager.Extensions;
using FoxyWebAppManager.Helpers;
using FoxyWebAppManager.Models;

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
    public partial FireFoxData FireFoxData { get; set; } = FireFoxDataExtensions.GetSavedFireFoxData();

    [ObservableProperty]
    public partial string WebHost { get; set; }

    [ObservableProperty]
    public partial bool IsCustomizeUserStyle { get; set; }

    [RelayCommand]
    private async Task OpenFireFoxPath() => await this.OpenFireFoxPathEx();

    [RelayCommand(CanExecute =nameof(CanWebAppSaveExecute))]
    private async Task OpenFavIconFromPath() => await this.OpenIconPathEx();

    [RelayCommand(CanExecute = nameof(CanWebAppSaveExecute))]
    private async Task SaveWebApp() => await SelectedFireFoxProfile.CreateWebApp(new Uri(WebHost.ToUriSchemeString()), FireFoxData.Path, FavIcon);

    [RelayCommand]
    private void SwitchUserChrome(bool activate) =>
    new FireFoxCssHelper(SelectedFireFoxProfile).ActivateUserChrome(activate);

    partial void OnWebHostChanged(string value)
      => _ = this.ChangeFavIconByWebHostChanged();

    public override void OnNavigatedFrom(){}

    public override void OnNavigatedTo(object parameter)
        => FoxProfiles =Helpers.FireFoxIniParser.LoadProfilesFromInstalledFF();

    partial void OnSelectedFireFoxProfileChanged(FireFoxProfile value) 
        => IsCustomizeUserStyle = new FireFoxCssHelper(SelectedFireFoxProfile).IsUserChromeActive;

    partial void OnFoxProfilesChanged(List<FireFoxProfile> value)
    {
        if (value.Count > 0)
        {
            SelectedFireFoxProfile = value.First();
        }
    }

}

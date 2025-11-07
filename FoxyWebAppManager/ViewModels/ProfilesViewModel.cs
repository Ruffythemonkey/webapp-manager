using CommunityToolkit.Mvvm.Input;
using FoxyWebAppManager.Helpers;
using FoxyWebAppManager.Models;
using FoxyWebAppManager.Extensions;

namespace FoxyWebAppManager.ViewModels;

public partial class ProfilesViewModel() : BaseViewModel
{
    public RangeObservableCollection<FireFoxProfile> FireFoxProfiles { get; set; } = [];

    [RelayCommand]
    private async Task RemoveProfileUI(FireFoxProfile foxProfile) => await this.RemoveProfile(foxProfile);

    [RelayCommand]
    private async Task CreateProfileUI() => await this.CreateProfile();

    [RelayCommand]
    private async Task OpenRoamingProfile(FireFoxProfile profile) => await profile.OpenRoamingFolder();

    public override void OnNavigatedFrom(){}

    public override void OnNavigatedTo(object parameter)
       => FireFoxProfiles.AddRange(FireFoxIniParser.LoadProfilesFromInstalledFF());

}

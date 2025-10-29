using CommunityToolkit.Mvvm.ComponentModel;
using FoxyWebAppManager.Collections;
using FoxyWebAppManager.Helpers;
using FoxyWebAppManager.Models;

namespace FoxyWebAppManager.ViewModels;

public partial class ProfilesViewModel() : BaseViewModel
{

    public RangeObservableCollection<FireFoxProfile> FireFoxProfiles { get; set; } = [];

    [ObservableProperty]
    public partial FireFoxProfile FireFoxProfileSelected { get; set; }

    [ObservableProperty]
    public partial string Watchlock {  get; set; }

    public override void OnNavigatedFrom(){}

    public override void OnNavigatedTo(object parameter)
       => FireFoxProfiles.AddRange(FireFoxIniParser.LoadProfilesFromInstalledFF());

}

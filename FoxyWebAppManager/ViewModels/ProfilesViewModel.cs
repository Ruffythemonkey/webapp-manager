using CommunityToolkit.Mvvm.ComponentModel;
using FoxyWebAppManager.Collections;
using FoxyWebAppManager.Extensions;
using FoxyWebAppManager.Helpers;
using FoxyWebAppManager.Models;
using System.Collections.ObjectModel;

namespace FoxyWebAppManager.ViewModels;

public partial class ProfilesViewModel : BaseViewModel
{

    public RangeObservableCollection<FireFoxProfile> FireFoxProfiles { get; set; } = [];

    [ObservableProperty]
    public partial FireFoxProfile FireFoxProfileSelected { get; set; }




    public override void OnNavigatedFrom()
    {
        //throw new NotImplementedException();
    }

    public override void OnNavigatedTo(object parameter)
      =>  FireFoxProfiles.AddRange(FireFoxIniParser.LoadProfilesFromInstalledFF());


}

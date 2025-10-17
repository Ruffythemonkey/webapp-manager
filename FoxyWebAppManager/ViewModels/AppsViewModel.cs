using CommunityToolkit.Mvvm.ComponentModel;
using FoxyWebAppManager.Models;
using static FoxyWebAppManager.Helpers.FireFoxIniParser;
using FoxyWebAppManager.Extensions;

namespace FoxyWebAppManager.ViewModels;

public partial class AppsViewModel : BaseViewModel
{

    [ObservableProperty]
    public partial List<FireFoxProfile> FoxProfiles { get; set; }

    [ObservableProperty]
    public partial FireFoxProfile SelectedProfil {  get; set; }

    [ObservableProperty]
    public partial FireFoxTaskbarJson WebApps { get; set; }

    [ObservableProperty]
    public partial TaskbarTab SelectedWebApp { get; set; }


    partial void OnFoxProfilesChanged(List<FireFoxProfile> value)
    {
        if (value.Count > 0)
        {
            SelectedProfil ??= value.First();
        }
    }

    partial void OnSelectedProfilChanged(FireFoxProfile value)
    {
        if (value is FireFoxProfile p)
        {
            WebApps = p.GetMainFolder().GetJson();
        }
    }

    partial void OnWebAppsChanged(FireFoxTaskbarJson value)
    {
        if (value is FireFoxTaskbarJson json)
        {
            SelectedWebApp = json.taskbarTabs.First();
        }
    }

    public override void OnNavigatedFrom()
    {
        //throw new NotImplementedException();
    }

    public override void OnNavigatedTo(object parameter)
        => FoxProfiles ??= IniReaderFireFox.LoadProfilesFromInstalledFF();
}

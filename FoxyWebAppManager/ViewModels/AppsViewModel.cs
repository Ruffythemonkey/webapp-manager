using CommunityToolkit.Mvvm.ComponentModel;
using FoxyWebAppManager.Models;
using FoxyWebAppManager.Extensions;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace FoxyWebAppManager.ViewModels;

public partial class AppsViewModel : BaseViewModel
{

    [ObservableProperty]
    public partial List<FireFoxProfile> FoxProfiles { get; set; }

    [ObservableProperty]
    public partial FireFoxProfile SelectedProfil {  get; set; }

    [ObservableProperty]
    public partial ObservableCollection<TaskbarTab> WebApps { get; set; } = [];

    [ObservableProperty]
    public partial TaskbarTab SelectedWebApp { get; set; }

    [ObservableProperty]
    public partial string Icon { get; set; } = "/Assets/WindowIcon.ico";

    [RelayCommand]
    private async Task PickIcon() => Icon = await SelectedWebApp.SetIconAsync(SelectedProfil); 

    [RelayCommand]
    private void UpdateWebApp() => SelectedWebApp.UpdateWebApp(Icon, SelectedProfil);

    [RelayCommand]
    private void RemoveWebAppUi(TaskbarTab item) => this.RemoveWebApp(item, SelectedProfil);

    partial void OnFoxProfilesChanged(List<FireFoxProfile> value)
    {
        if (value.Count > 0)
        {
            SelectedProfil = value.FirstOrDefault() ?? null!;
        }
    }

    partial void OnSelectedProfilChanged(FireFoxProfile value)
    {
        if (value is FireFoxProfile p)
        {
            WebApps.Clear();
            foreach (var item in p.GetMainFolder().GetJson().GetAvailableWebApps())
            {
                WebApps.Add(item);
            }
            SelectedWebApp = WebApps.FirstOrDefault()!;
            /*p.GetMainFolder().GetJson().GetAvailableWebApps();*/
        }
    }

    //partial void OnWebAppsChanged(ObservableCollection<TaskbarTab> value)
    //{
    //    if (value is ObservableCollection<TaskbarTab> tabs)
    //    {
    //        _dispatcherQueue.TryEnqueue(() =>
    //        {
    //            SelectedWebApp = tabs.FirstOrDefault()!;
    //        });
    //    }
    //}

    partial void OnSelectedWebAppChanged(TaskbarTab value)
    {
        if (value is TaskbarTab tab)
        {
            Icon = tab.GetIcon(SelectedProfil);
        }
        else
        {
            Icon = "/Assets/WindowIcon.ico";
        }
    }

    public override void OnNavigatedFrom(){}

    public override void OnNavigatedTo(object parameter)
       => this.ProfilesWithWebApps();
}

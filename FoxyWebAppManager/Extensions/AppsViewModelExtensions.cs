using FoxyWebAppManager.Helpers;
using FoxyWebAppManager.Models;
using FoxyWebAppManager.ViewModels;

namespace FoxyWebAppManager.Extensions
{
    public static class AppsViewModelExtensions
    {
        extension(AppsViewModel vm)
        {
            public void ProfilesWithWebApps()
            {
                vm.FoxProfiles = FireFoxIniParser.LoadProfilesFromInstalledFF()
               .Where(x => x.GetMainFolder().GetJson().taskbarTabs.IsAnyTaskBarTabItemInStartMenu())
               .ToList();
               //is { Count: > 0 } apps ? apps : null!;
            }

            public void RemoveWebApp(TaskbarTab tab, FireFoxProfile profile)
            {
                if (vm.SelectedWebApp == tab)
                    vm.SelectedWebApp = vm.WebApps.FirstOrDefault(x => x != tab)!;

                tab.RemoveWebAppIO(profile);
                vm.WebApps.Remove(tab);

                if (vm.WebApps.Count == 0)
                    vm.ProfilesWithWebApps();
            }

        }
    }
}

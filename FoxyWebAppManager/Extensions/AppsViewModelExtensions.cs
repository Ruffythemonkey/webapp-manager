using FoxyWebAppManager.Models;
using FoxyWebAppManager.ViewModels;
using static FoxyWebAppManager.Helpers.FireFoxIniParser;

namespace FoxyWebAppManager.Extensions
{
    public static class AppsViewModelExtensions
    {
        extension(AppsViewModel vm)
        {
            public void ProfilesWithWebApps()
            { 
                vm.FoxProfiles = IniReaderFireFox.LoadProfilesFromInstalledFF()
               .Where(x => x.GetMainFolder().GetJson().taskbarTabs.IsAnyTaskBarTabItemInStartMenu())
               .ToList() 
               is { Count: > 0} apps ? apps : null!;
            }

            public void RemoveWebApp(TaskbarTab tab,FireFoxProfile profile)
            {
                if (vm.SelectedWebApp == tab)
                {
                    if (vm.WebApps.Count > 1)
                    {
                        vm.SelectedWebApp = vm.WebApps.First(x => x != tab);
                    }
                }

                tab.RemoveWebAppIO(profile);
                if (vm.WebApps.Count == 1)
                {
                    vm.ProfilesWithWebApps();
                }
                else
                {
                    vm.WebApps.Remove(tab);
                }
            }
     
        }
    }
}

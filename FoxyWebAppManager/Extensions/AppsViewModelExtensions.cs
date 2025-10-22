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
               is { Count: > 0} apps ? apps : vm.FoxProfiles;
            }
     
        }
    }
}

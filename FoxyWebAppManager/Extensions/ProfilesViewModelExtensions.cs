using FoxyWebAppManager.ContentDialogs;
using FoxyWebAppManager.Helpers;
using FoxyWebAppManager.Models;
using FoxyWebAppManager.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
namespace FoxyWebAppManager.Extensions
{
    public static class ProfilesViewModelExtensions
    {
        extension(ProfilesViewModel vm)
        {
            public async Task RemoveProfile(FireFoxProfile profile)
            {
                ContentDialog dialog = new DialogRemoveProfile().ContentDialog;

                if (await dialog.ShowAsync() == ContentDialogResult.Secondary)
                {
                    return;
                }


                //loads all Profiles and gives a List without the one removing Profile
                var keepProfiles = FireFoxIniParser
                    .LoadProfilesFromInstalledFF()
                    .Where(x => !x.Equals(profile))
                    .ToList();

                //write the FirefoxProfile.ini
                keepProfiles.AttachProfilesToIniFile();
                //Remove the selected FF Profile
                profile.RemoveProfileFolder();

                vm.FireFoxProfiles.ReloadFireFoxProfiles();
            }

            public async Task CreateProfile()
            {
                ContentDialog dialog = new DialogRenameProfile().ContentDialog;

                if (await dialog.ShowAsync() == ContentDialogResult.Primary
                    && dialog.Tag is string str
                    && !string.IsNullOrWhiteSpace(str))
                {   
                    //Create Process =>
                    str = str.Trim();
                    var p = FireFoxDataExtensions.GetSavedFireFoxData();
                    if (p is FireFoxData fire)
                    {
                        await fire.CreateProfile(str);
                        vm.FireFoxProfiles.ReloadFireFoxProfiles(vm._dispatcherQueue);
                    }
                }
            }

        }
    }
}

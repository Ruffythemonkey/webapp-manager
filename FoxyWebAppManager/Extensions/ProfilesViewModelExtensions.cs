using CommunityToolkit.WinUI;
using FoxyWebAppManager.ContentDialogs;
using FoxyWebAppManager.Helpers;
using FoxyWebAppManager.Models;
using FoxyWebAppManager.ViewModels;
using Microsoft.UI.Xaml.Controls;
namespace FoxyWebAppManager.Extensions
{
    public static class ProfilesViewModelExtensions
    {
        extension(ProfilesViewModel vm)
        {
            /// <summary>
            /// Try Remove Profile completely
            /// </summary>
            /// <param name="profile"></param>
            /// <returns></returns>
            /// <exception cref="FoxyException"></exception>
            public async Task RemoveProfile(FireFoxProfile profile)
            {

                try
                {

                    //Prof is Profile in Use, when throw FoxyException
                    if (profile.IsProfileInUse())
                    {
                        throw new FoxyException(string.Format("ProfileRemoveInUser"
                            .GetLocalized()!, profile.Name));
                    }

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
                catch (Exception ex)
                {

                    await ex.ShowMessageUIAsync();
                }
            }

            /// <summary>
            /// Create FF Profile with FireFox own Profile Creator
            /// </summary>
            /// <returns></returns>
            public async Task CreateProfile()
            {
                ContentDialog dialog = new DialogRenameProfile().ContentDialog;

                if (await dialog.ShowAsync() == ContentDialogResult.Primary
                    && dialog.Tag is string str
                    && !string.IsNullOrWhiteSpace(str))
                {   
                    //Create Process =>
                    str = str.Trim();
                    var p = App.Settings.FireFoxApp.GetSavedFireFoxData();
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

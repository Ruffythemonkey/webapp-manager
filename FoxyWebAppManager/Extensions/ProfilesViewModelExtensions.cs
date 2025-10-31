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
            public void RemoveProfile(FireFoxProfile profile)
            {
                var allProfiles = FireFoxIniParser
                    .LoadProfilesFromInstalledFF()
                    .Where(x=> !x.Equals(profile))
                    .ToList();

                allProfiles.AttachProfilesToIniFile();
                profile.RemoveProfileFolder();

                vm.FireFoxProfiles.Clear();
                vm.FireFoxProfiles.AddRange(allProfiles);
            }

            public async Task CreateProfile()
            {
                ContentDialog dialog = new ContentDialog()
                {
                    PrimaryButtonText = "ProfileCreateYes".GetLocalized(),
                    SecondaryButtonText = "Cancel".GetLocalized(),
                    XamlRoot = App.MainWindow.Content.XamlRoot,
                    Content = new DialogRenameProfile()
                };

                if (await dialog.ShowAsync() == ContentDialogResult.Primary
                    && dialog.Content is DialogRenameProfile profile
                    && profile.ProfileName is string str
                    && !string.IsNullOrWhiteSpace(str))
                {
                    str = str.Trim();
                    var p = FireFoxDataExtensions.GetSavedFireFoxData();
                    if (p is FireFoxData fire)
                    {
                       await fire.CreateProfile(str);

                        vm._dispatcherQueue.TryEnqueue(() => 
                        {
                            vm.FireFoxProfiles.Clear();
                            vm
                            .FireFoxProfiles
                            .AddRange(FireFoxIniParser.LoadProfilesFromInstalledFF());
                        });
                        

                    }
                }
            }

        }
    }
}

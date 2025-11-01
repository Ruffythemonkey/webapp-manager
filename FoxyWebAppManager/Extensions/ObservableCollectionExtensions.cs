using FoxyWebAppManager.Collections;
using FoxyWebAppManager.Models;
using Microsoft.UI.Dispatching;

namespace FoxyWebAppManager.Extensions
{
    public static class ObservableCollectionExtensions
    {
        extension(RangeObservableCollection<FireFoxProfile> profiles)
        {
            /// <summary>
            /// Reload FireFox Profiles in this Collection, clears before
            /// </summary>
            public void ReloadFireFoxProfiles()
                  => profiles.ClearAndAdd(Helpers.FireFoxIniParser.LoadProfilesFromInstalledFF());

            /// <summary>
            /// Reload FireFox Profiles in this Collection, clears before. And uses 
            /// DispatcherQueue in async Operations
            /// </summary>
            /// <param name="queue"></param>
            public void ReloadFireFoxProfiles(DispatcherQueue queue)
                => queue.TryEnqueue(() => profiles.ReloadFireFoxProfiles());
        }

    }
}

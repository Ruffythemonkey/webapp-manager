using FoxyWebAppManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxyWebAppManager.Extensions
{
    public static class ListExtensions
    {
        //extension(List<FireFoxProfile> foxProfiles)
        //{
        //    public void ReloadProfiles()
        //        => foxProfiles = Helpers.FireFoxIniParser.LoadProfilesFromInstalledFF();
        //}
        public static void ReloadProfiles(this List<FireFoxProfile> profiles)
            => _ReloadProfiles(ref profiles);

        private static void _ReloadProfiles(ref List<FireFoxProfile> profiles)
            => profiles = Helpers.FireFoxIniParser.LoadProfilesFromInstalledFF();
    }
}

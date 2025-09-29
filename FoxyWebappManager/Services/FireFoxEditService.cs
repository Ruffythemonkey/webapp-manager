using System.Text.Json;
using FoxyWebappManager.Extensions;
using FoxyWebappManager.Models;

namespace FoxyWebappManager.Services
{
    public class FireFoxEditService
    {


        public void CreateWebApp(FireFoxProfile profile,Uri url, string firfoxPath, string iconPath)
        {
            var args = profile
                .GetMainFolder()
                .SetJson(url)
                .GetWindowsLnk(url);

            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var name = Path.Combine(desktop, url.GetDomainNameWithoutExtension()); 
            Helpers.IOHelper.CreateShortcut(name ,firfoxPath, args, iconPath);

        }


        public void AcitvateChromeStyle(FireFoxProfile profile, bool activate)
        {

        }


    }
}

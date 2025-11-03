using FoxyWebAppManager.Models;

namespace FoxyWebAppManager.Extensions
{
    public static class AppSettingsExtensions
    {
        private static AppSettings? _instance = null!;

        /// <summary>
        /// Save All Application Data 
        /// </summary>
        /// <param name="settings"></param>
        public static void Save(this AppSettings settings)
           => LocalDataAppPath.Save<AppSettings>(settings);

        /// <summary>
        /// Load All Application Data
        /// </summary>
        /// <returns></returns>
        public static AppSettings Load()
            => _instance ?? LocalDataAppPath.Read<AppSettings>() ?? new AppSettings();
    }
}

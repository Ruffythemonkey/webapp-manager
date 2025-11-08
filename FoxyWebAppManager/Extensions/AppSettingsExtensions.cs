using FoxyWebAppManager.Models;

namespace FoxyWebAppManager.Extensions
{
    public static class AppSettingsExtensions
    {
        private static  AppSettings _instance = null!;

        public static AppSettings GetSettings { get => Load(); }


        /// <summary>
        /// Save All Application Data 
        /// </summary>
        /// <param name="settings"></param>
        public static void Save(this AppSettings settings)
        {
            if (settings is not null && _instance is not null)
            {
                LocalDataAppPath.Save<AppSettings>(settings);
            } 
        }

        /// <summary>
        /// Load All Application Data
        /// </summary>
        /// <returns></returns>
        private static AppSettings Load()
           => _instance ??= LocalDataAppPath.Read<AppSettings>() ?? new AppSettings();
    }
}

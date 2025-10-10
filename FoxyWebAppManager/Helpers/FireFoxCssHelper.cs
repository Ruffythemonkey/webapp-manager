using FoxyWebAppManager.Extensions;
using FoxyWebAppManager.Models;

namespace FoxyWebappManager.Helpers
{
    public class FireFoxCssHelper(FireFoxProfile profile)
    {

        private readonly string _legacyUserCustomization = "toolkit.legacyUserProfileCustomizations.stylesheets";

        private readonly List<string> _ChromeSettings = new UserChrome().UserChromeSettings;

        public string PrefsPath
        {
            get => Path.Combine(profile.GetMainFolder().ProfielPath, "prefs.js");
        }

        public string UserChromeFile
        {
            get => Path.Combine(profile.GetMainFolder().ProfielPath, @"chrome\userChrome.css");
        }

        public bool UserChromeFileExist
        {
            get => File.Exists(UserChromeFile);
        }

        public bool IsPrefsExist
        {
            get {
                return File.Exists(PrefsPath);
            }
        }

        public bool IsUserCustomizationActive
        {
            get {
                if (!IsPrefsExist)
                    return false;

                var match = File.ReadLines(PrefsPath)
                    .FirstOrDefault(line =>
                        line.Contains(_legacyUserCustomization, StringComparison.InvariantCultureIgnoreCase));

                return match?.Contains("true", StringComparison.InvariantCultureIgnoreCase) == true;
            }
        }

        public bool IsUserChromeActive
        {
            get {
                if (!UserChromeFileExist)
                    return false;

                var fr = File.ReadAllText(UserChromeFile);

                var test = _ChromeSettings
                    .Where(x => fr.Contains(x))
                    .ToList();

                return _ChromeSettings.Count == test.Count;
            }
        }

        private void ActivateUserCustomization()
        {
            const string prefLine = "user_pref(\"toolkit.legacyUserProfileCustomizations.stylesheets\", true);";

            if (IsUserCustomizationActive)
                return;

            if (IsPrefsExist)
            {
                var fr = File.ReadAllLines(PrefsPath).ToList();
                int index = fr.FindIndex(x => x.Contains(_legacyUserCustomization, StringComparison.InvariantCultureIgnoreCase));

                if (index >= 0)
                {
                    fr[index] = prefLine; // vorhandene Zeile aktualisieren
                }
                else
                {
                    fr.Add(prefLine); // Zeile hinzufügen
                }

                File.WriteAllLines(PrefsPath, fr);
                return;
            }

            File.WriteAllText(PrefsPath, prefLine); // Datei neu erstellen
        }

        private void CreateUserChromeFile()
        {
            if (!UserChromeFileExist)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(UserChromeFile)!);
                var fc = File.Create(UserChromeFile);
                fc.Close();
            }
        }

        public void ActivateUserChrome(bool activate)
        {
            CreateUserChromeFile();
            if (activate)
            {
                ActivateUserCustomization();
                AddSettings();
            }
            else
            {
                RemoveSettings();
            }
        }

        private string RemoveSettings()
        {
            var fr = File.ReadAllText(UserChromeFile);
            foreach (var item in _ChromeSettings)
            {
                fr = fr.Replace(item, "");
            }
            File.WriteAllText(UserChromeFile, fr);
            return fr;
        }

        private void AddSettings()
        {
            var str = RemoveSettings();
            foreach (var item in _ChromeSettings)
            {
                str += $"\n{item}";
            }
            File.WriteAllText(UserChromeFile, str);
        }

    }
}
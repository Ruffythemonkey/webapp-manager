using FoxyWebappManager.Models;

namespace FoxyWebappManager.Helpers;

public class FireFoxIniParser
{

   

    public class IniReaderFireFox
    {
        public static List<FireFoxProfile> LoadProfilesFromInstalledFF()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appData = Path.Combine(appData, "Mozilla/Firefox/profiles.ini");
            if (File.Exists(appData)) 
            {
                return LoadProfiles(appData);
            }
            return new List<FireFoxProfile>();
        }

        public static List<FireFoxProfile> LoadProfiles(string file)
        {
            var profiles = new List<FireFoxProfile>();
            FireFoxProfile? current = null;

            foreach (var rawLine in File.ReadAllLines(file))
            {
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                    continue;

                // Neue Section erkannt
                if (line.StartsWith("[") && line.EndsWith("]"))
                {

                    if (current != null)
                        profiles.Add(current);

                    current = new FireFoxProfile { Section = line[1..^1] };
                }
                else if (current != null && line.Contains('='))
                {
                    var parts = line.Split('=', 2);
                    var key = parts[0].Trim().ToLower();
                    var value = parts[1].Trim();

                    if (key == "name") current.Name = value;
                    if (key == "path") current.Path = value;
                    if (key == "isrelative") current.IsRelative = value == "1" ? true : false;
                }
            }

            if (current != null)
                profiles.Add(current);

            return profiles
                .Where(x => !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.Path))
                .ToList();
        }
    }

}

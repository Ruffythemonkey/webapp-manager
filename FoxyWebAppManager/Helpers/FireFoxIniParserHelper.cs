using FoxyWebAppManager.Models;

namespace FoxyWebAppManager.Helpers;

public static class FireFoxIniParser
{

    public static string FireFoxProfileIniPath
    {
        get 
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, "Mozilla/Firefox/profiles.ini");
        }
    }

    public static List<FireFoxProfile> LoadProfilesFromInstalledFF()
    {
        if (File.Exists(FireFoxProfileIniPath))
            return LoadProfiles(FireFoxProfileIniPath);
        
        return [];
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
                if (key == "isrelative") current.IsRelative = value == "1";
            }
        }

        if (current != null)
            profiles.Add(current);

        return profiles
            .Where(x => !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.Path))
            .ToList();
    }

    private static List<string> RemoveAllIniProfiles()
    {
        if (!File.Exists(FireFoxProfileIniPath))
            return [];

        bool insideSection = false;
        List<string> updateLines = [];

        foreach (var rawLine in File.ReadAllLines(FireFoxProfileIniPath))
        {
            var line = rawLine.Trim();
            
            if(line.StartsWith("[") && line.EndsWith("]"))
            {
                insideSection = line[1..^1].StartsWith("Profile"); 
            }

            if (!insideSection)
            {
                updateLines.Add(line);
            }
        }

        return updateLines;
        //return string.Join("\n",updateLines);
    }

    /// <summary>
    /// Modify the profiles Ini so that only those that are already present remain.
    /// </summary>
    /// <param name="profiles"></param>
    public static void AttachProfilesToIniFile(this List<FireFoxProfile> profiles)
    {
        if (!File.Exists(FireFoxProfileIniPath))
            return;

        var clearIni = RemoveAllIniProfiles();
        foreach (var profile in profiles) 
        {
            if (clearIni.Last() != "")
            {
                clearIni.Add("");
            }
            clearIni.Add($"[{profile.Section}]");
            clearIni.Add($"Name={profile.Name}");
            clearIni.Add($"IsRelative={(profile.IsRelative ? 1 : 0)}");
            clearIni.Add($"Path={profile.Path}");
        }

        File.WriteAllLines(FireFoxProfileIniPath, clearIni);

    }

}
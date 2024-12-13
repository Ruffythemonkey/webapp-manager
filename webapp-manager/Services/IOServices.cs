using IWshRuntimeLibrary;

namespace webapp_manager.Services
{
    public static class IOServices
    {
        public static void CopyFolder(string sourceFolder, string destinationFolder)
        {
            // Zielordner erstellen, falls er nicht existiert
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            // Alle Dateien im Quellordner kopieren
            foreach (string file in Directory.GetFiles(sourceFolder))
            {
                // Zielpfad für die Datei
                string destFile = Path.Combine(destinationFolder, Path.GetFileName(file));
                System.IO.File.Copy(file, destFile, true); // Überschreiben erzwingen (true)
            }

            // Alle Unterordner im Quellordner kopieren
            foreach (string folder in Directory.GetDirectories(sourceFolder))
            {
                // Zielpfad für den Unterordner
                string destFolder = Path.Combine(destinationFolder, Path.GetFileName(folder));
                CopyFolder(folder, destFolder); // Rekursive Kopie
            }
        }

        /// <summary>
        /// Create a Windows-compatible Shortcut
        /// </summary>
        /// <param name="shortcutPath">Save path to Shortcut, name it .lnk</param>
        /// <param name="target">File or application to link to</param>
        /// <param name="args">Arguments to pass to the target</param>
        /// <param name="icon">Path to an icon file (.ico)</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool CreateShortcut(string shortcutPath, string target, string? args, string? icon)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(shortcutPath) || string.IsNullOrWhiteSpace(target))
                    throw new ArgumentException("Shortcut path and target path must not be null or empty.");

                if (!shortcutPath.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
                {
                    shortcutPath += ".lnk";
                }

                if (!string.IsNullOrWhiteSpace(icon) && !icon.EndsWith(".ico", StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("Icon must be a .ico file.");
                }

                // Create the shortcut
                WshShell shell = new();

                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

                if (!string.IsNullOrEmpty(args))
                {
                    shortcut.Arguments = args;
                }

                if (!string.IsNullOrEmpty(icon))
                {
                    shortcut.IconLocation = icon;
                }

                shortcut.TargetPath = target;
                shortcut.WorkingDirectory = Path.GetDirectoryName(target);
                shortcut.Save();


                return true; // Erfolgreich
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}

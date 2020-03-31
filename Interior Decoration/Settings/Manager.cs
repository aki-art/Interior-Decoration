using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace InteriorDecoration.Settings
{
    class SettingsManager
    {
        private const string MODS = "mods";
        private const string SETTINGS = "settings";
        private const string FOLDERNAME = "InteriorDecoration";
        private const string FILE_NAME = "config.json";

        public static string localPath;
        public static string exteriorPath;

        public static bool isThereAnOutsideConfig = false;
        public static bool isThereALocalConfig = false;


        public static void Initialize()
        {
            localPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            exteriorPath = GetDirectory();

            isThereAnOutsideConfig = File.Exists(Path.Combine(localPath, FILE_NAME));
            isThereAnOutsideConfig = File.Exists(Path.Combine(exteriorPath, FILE_NAME));

            Log.Debuglog("Config save folder in case of local: " + localPath);
            Log.Debuglog("Config save folder in case of exterior: " + exteriorPath);

            LoadSettings();
        }

        public static void LoadSettings()
        {
            Mod.Settings = new UserSettings();

            if (isThereAnOutsideConfig)
                Mod.Settings = LoadSettingsFromFile(exteriorPath);
            else if (isThereALocalConfig)
                Mod.Settings = LoadSettingsFromFile(localPath);

        }

        public static void SaveSettings()
        {

            if (!Mod.Settings.UseSafeFolder)
            {
                if (isThereAnOutsideConfig)
                    RemoveModSettingsFolder();
                WriteSettingsToFile(localPath);
            }
            else
                WriteSettingsToFile(exteriorPath);

            isThereAnOutsideConfig = File.Exists(Path.Combine(localPath, FILE_NAME));
            isThereAnOutsideConfig = File.Exists(Path.Combine(exteriorPath, FILE_NAME));
        }

        public static void WriteSettingsToFile(string path)
        {
            var filePath = Path.Combine(path, FILE_NAME);
            try
            {
                if (!Directory.Exists(path) && path == exteriorPath)
                    Directory.CreateDirectory(path);

                using (var sw = new StreamWriter(filePath))
                {
                    var serializedUserSettings = JsonConvert.SerializeObject(Mod.Settings, Formatting.Indented);
                    sw.Write(serializedUserSettings);
                    Log.Info($"Settings saved to: {filePath}");
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't write to {filePath}, {e.Message}");
            }

        }
        private static void RemoveModSettingsFolder()
        {
            // .../Klei/OxygenNotIncluded/mods/settings 
            string settingsPath = Path.Combine(Util.RootFolder(), MODS, SETTINGS);
            DirectoryInfo settingsDir = new DirectoryInfo(settingsPath);

            Log.Info($"User changed config save location. Cleaning up files...");

            try
            {
                Directory.Delete(exteriorPath, true); // Removes Asphalt Tiles folder
                Log.Info($"Removed folder at {exteriorPath}.");
                Log.Info($"Attempting to remove settings folder...");

                // Deletes this DirectoryInfo if it is empty.
                // If other mod files are present, it will leave them alone
                settingsDir.Delete(recursive: false);

                if (!Directory.Exists(settingsPath))
                    Log.Info($"Removed folder at {settingsPath}");
            }
            catch
            {
            }

            if (Directory.Exists(settingsPath))
                Log.Info("Other mods were using this folder, (or something else went wrong), leaving it alone.");
        }


        private static UserSettings LoadSettingsFromFile(string path)
        {
            var filePath = Path.Combine(path, FILE_NAME);
            Log.Debuglog("Loading config files from: " + filePath);
            UserSettings userSettings = new UserSettings();

            try
            {
                using (var r = new StreamReader(filePath))
                {
                    var json = r.ReadToEnd();
                    userSettings = JsonConvert.DeserializeObject<UserSettings>(json);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't read {filePath}, {e.Message}. Using default settings.");
                return new UserSettings();
            }

            return userSettings;
        }
        private static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        public static string GetDirectory()
        {
            return Path.Combine(Util.RootFolder(), MODS, SETTINGS, FOLDERNAME);
        }

    }
}

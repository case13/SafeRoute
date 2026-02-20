using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SafeRoute.Revit.NetFramework.Settings
{
    public static class PluginSettingsProvider
    {
        private static string GetFilePath()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SafeRoute");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return Path.Combine(folder, "settings.json");
        }

        public static PluginSettings Load()
        {
            try
            {
                var path = GetFilePath();
                if (!File.Exists(path))
                    return new PluginSettings();

                var json = File.ReadAllText(path, Encoding.UTF8);
                var settings = JsonConvert.DeserializeObject<PluginSettings>(json);

                return settings ?? new PluginSettings();
            }
            catch
            {
                return new PluginSettings();
            }
        }

        public static void Save(PluginSettings settings)
        {
            try
            {
                var path = GetFilePath();
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(path, json, Encoding.UTF8);
            }
            catch
            {
            }
        }
    }
}

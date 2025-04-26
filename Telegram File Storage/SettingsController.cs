using System;
using System.IO;
using System.Text.Json;
using TelegramFileStorage.Models;

namespace TelegramFileStorage
{
    public static class SettingsController
    {
        private static readonly string SettingsFile = Path.Combine(AppContext.BaseDirectory, "settings.json");
        public static SettingsModel Current { get; private set; } = new SettingsModel();

        public static void Load()
        {
            if (!File.Exists(SettingsFile))
            {
                Current = new SettingsModel();
                Save();
                return;
            }
            var json = File.ReadAllText(SettingsFile);
            var model = JsonSerializer.Deserialize<SettingsModel>(json);
            Current = model ?? new SettingsModel();
        }

        public static void Save()
        {
            var json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFile, json);
        }
    }
}

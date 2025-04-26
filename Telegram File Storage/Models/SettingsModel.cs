using System.Text.Json.Serialization;

namespace TelegramFileStorage.Models
{
    public class SettingsModel
    {
        public bool IsDarkTheme { get; set; } = true;
        public string TelegramToken { get; set; } = string.Empty;
        public string ChannelId { get; set; } = string.Empty;
        public bool SyncEnabled { get; set; }
        public bool MultiUploadEnabled { get; set; }
        public int UploadThreads { get; set; } = 1;
        public int UploadSpeedLimit { get; set; }
        public bool MultiDownloadEnabled { get; set; }
        public int DownloadThreads { get; set; } = 1;
        public int DownloadSpeedLimit { get; set; }
    }
}

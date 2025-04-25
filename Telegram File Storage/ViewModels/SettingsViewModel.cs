using System;
using System.ComponentModel;

namespace TelegramFileStorage.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public enum SettingsCategory
        {
            Main,
            Telegram,
            Sync,
            Upload,
            Download
        }

        private SettingsCategory _selectedCategory = SettingsCategory.Main;
        public SettingsCategory SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); }
        }
        public SettingsCategory[] Categories { get; } = (SettingsCategory[])Enum.GetValues(typeof(SettingsCategory);

        private string _telegramToken;
        public string TelegramToken
        {
            get => _telegramToken;
            set { _telegramToken = value; OnPropertyChanged(); }
        }

        private string _channelId;
        public string ChannelId
        {
            get => _channelId;
            set { _channelId = value; OnPropertyChanged(); }
        }

        private bool _syncEnabled;
        public bool SyncEnabled
        {
            get => _syncEnabled;
            set { _syncEnabled = value; OnPropertyChanged(); }
        }

        private int _maxUploadSize;
        public int MaxUploadSize
        {
            get => _maxUploadSize;
            set { _maxUploadSize = value; OnPropertyChanged(); }
        }
    }
}

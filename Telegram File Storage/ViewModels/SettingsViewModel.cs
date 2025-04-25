using System.ComponentModel;

namespace TelegramFileStorage.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
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

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using TelegramFileStorage.Models;

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

        private static readonly string SettingsFile = Path.Combine(AppContext.BaseDirectory, "settings.json");

        private SettingsCategory _selectedCategory = SettingsCategory.Main;
        public SettingsCategory SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); }
        }
        public SettingsCategory[] Categories { get; } = (SettingsCategory[])Enum.GetValues(typeof(SettingsCategory));

        private string _telegramToken = string.Empty;
        public string TelegramToken
        {
            get => _telegramToken;
            set { _telegramToken = value; OnPropertyChanged(); }
        }

        private string _channelId = string.Empty;
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

        private bool _isDarkTheme = true;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set { _isDarkTheme = value; OnPropertyChanged(); }
        }

        private int _themeIndex = 1; // 0 — светлая, 1 — тёмная
        public int ThemeIndex
        {
            get => _themeIndex;
            set {
                _themeIndex = value;
                IsDarkTheme = _themeIndex == 1;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDarkTheme));
            }
        }

        private bool _multiUploadEnabled;
        public bool MultiUploadEnabled
        {
            get => _multiUploadEnabled;
            set { _multiUploadEnabled = value; OnPropertyChanged(); }
        }

        private int _uploadThreads = 1;
        public int UploadThreads
        {
            get => _uploadThreads;
            set { _uploadThreads = value; OnPropertyChanged(); }
        }

        private int _uploadSpeedLimit;
        public int UploadSpeedLimit
        {
            get => _uploadSpeedLimit;
            set { _uploadSpeedLimit = value; OnPropertyChanged(); }
        }

        private bool _multiDownloadEnabled;
        public bool MultiDownloadEnabled
        {
            get => _multiDownloadEnabled;
            set { _multiDownloadEnabled = value; OnPropertyChanged(); }
        }

        private int _downloadThreads = 1;
        public int DownloadThreads
        {
            get => _downloadThreads;
            set { _downloadThreads = value; OnPropertyChanged(); }
        }

        private int _downloadSpeedLimit;
        public int DownloadSpeedLimit
        {
            get => _downloadSpeedLimit;
            set { _downloadSpeedLimit = value; OnPropertyChanged(); }
        }

        private string _cardBackground = string.Empty;
        public string CardBackground
        {
            get => _cardBackground;
            set { _cardBackground = value; OnPropertyChanged(); }
        }

        private bool _welcomeShown;
        public bool WelcomeShown
        {
            get => _welcomeShown;
            set { _welcomeShown = value; OnPropertyChanged(); }
        }

        public ICommand? ShowFilesPageCommand { get; set; }
        public ICommand SaveTelegramCommand { get; set; }
        public ICommand SaveSyncCommand { get; set; }
        public ICommand SaveUploadCommand { get; set; }
        public ICommand SaveDownloadCommand { get; set; }
        public ICommand ChangeThemeCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public SettingsViewModel()
        {
            SaveTelegramCommand = new RelayCommand(SaveSettings);
            SaveSyncCommand = new RelayCommand(SaveSettings);
            SaveUploadCommand = new RelayCommand(SaveSettings);
            SaveDownloadCommand = new RelayCommand(SaveSettings);
            ChangeThemeCommand = new RelayCommand(() => { SaveSettings(); ApplyTheme(); });
            ChangePasswordCommand = new RelayCommand(() => { /* TODO: Реализовать смену пароля */ });
            LogoutCommand = new RelayCommand(() => { /* TODO: Реализовать выход */ });
            // Инициализация свойств из SettingsController
            var model = SettingsController.Current;
            IsDarkTheme = model.IsDarkTheme;
            TelegramToken = model.TelegramToken;
            ChannelId = model.ChannelId;
            SyncEnabled = model.SyncEnabled;
            MultiUploadEnabled = model.MultiUploadEnabled;
            UploadThreads = model.UploadThreads;
            UploadSpeedLimit = model.UploadSpeedLimit;
            MultiDownloadEnabled = model.MultiDownloadEnabled;
            DownloadThreads = model.DownloadThreads;
            DownloadSpeedLimit = model.DownloadSpeedLimit;
            ThemeIndex = model.IsDarkTheme ? 1 : 0;
            WelcomeShown = model.WelcomeShown;
        }

        public void SaveSettings()
        {
            var model = SettingsController.Current;
            model.IsDarkTheme = ThemeIndex == 1;
            model.TelegramToken = TelegramToken;
            model.ChannelId = ChannelId;
            model.SyncEnabled = SyncEnabled;
            model.MultiUploadEnabled = MultiUploadEnabled;
            model.UploadThreads = UploadThreads;
            model.UploadSpeedLimit = UploadSpeedLimit;
            model.MultiDownloadEnabled = MultiDownloadEnabled;
            model.DownloadThreads = DownloadThreads;
            model.DownloadSpeedLimit = DownloadSpeedLimit;
            model.WelcomeShown = WelcomeShown;
            SettingsController.Save();
        }

        public void LoadSettings() { /* больше не нужен, всё через SettingsController */ }

        private void SetCardBackgroundByTheme()
        {
            var theme = Avalonia.Application.Current?.ActualThemeVariant;
            CardBackground = theme == Avalonia.Styling.ThemeVariant.Dark ? "#232323" : "#FFF";
        }

        private void ApplyTheme()
        {
            if (Avalonia.Application.Current is { } app)
            {
                app.RequestedThemeVariant = IsDarkTheme
                    ? Avalonia.Styling.ThemeVariant.Dark
                    : Avalonia.Styling.ThemeVariant.Light;
            }
            SetCardBackgroundByTheme();
        }
    }
}

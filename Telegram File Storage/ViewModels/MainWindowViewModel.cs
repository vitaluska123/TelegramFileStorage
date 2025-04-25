using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Models;

namespace TelegramFileStorage.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<FileModel> Files { get; set; } = new();
        public string CurrentPath { get; set; } = "/";

        public enum MainPageType { Files, Settings }

        private MainPageType _currentPage = MainPageType.Files;
        public MainPageType CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(); }
        }

        public ICommand DownloadCommand { get; }
        public ICommand UploadCommand { get; }
        public ICommand MoveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand ShowFilesPageCommand => new RelayCommand(ShowFilesPage);

        public event Action? OpenSettingsRequested;

        public SettingsViewModel Settings { get; } = new SettingsViewModel();

        public MainWindowViewModel()
        {
            DownloadCommand = new RelayCommand(OnDownload);
            UploadCommand = new RelayCommand(OnUpload);
            MoveCommand = new RelayCommand(OnMove);
            DeleteCommand = new RelayCommand(OnDelete);
            SettingsCommand = new RelayCommand(OnSettings);

            // Пример данных для отображения
            Files.Add(new FileModel { Name = "example.txt", VirtualPath = "/", Size = 12345, Hash = "abc123", IsDeleted = false });
        }

        private void OnDownload() { }
        private void OnUpload() { }
        private void OnMove() { }
        private void OnDelete() { }
        private void OnSettings() {
            CurrentPage = MainPageType.Settings;
        }
        public void ShowFilesPage() {
            CurrentPage = MainPageType.Files;
        }
    }
}

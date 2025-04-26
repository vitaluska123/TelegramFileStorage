using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Models;

namespace TelegramFileStorage.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainPageViewModel MainPageViewModel { get; }
        public SettingsViewModel SettingsPageViewModel { get; } = new SettingsViewModel();
        public WelcomePageViewModel WelcomePageViewModel { get; } = new WelcomePageViewModel();

        private ViewModelBase _currentPageVm;
        public ViewModelBase CurrentPageVm
        {
            get => _currentPageVm;
            set { _currentPageVm = value; OnPropertyChanged(); }
        }

        public MainWindowViewModel()
        {
            SettingsPageViewModel.ShowFilesPageCommand = new RelayCommand(ShowFilesPage);
            MainPageViewModel = new MainPageViewModel(OnSettings);
            WelcomePageViewModel.WelcomeFinished += OnWelcomeFinished;
            // Показываем приветствие только если WelcomeShown == false
            if (!SettingsController.Current.WelcomeShown)
                _currentPageVm = WelcomePageViewModel;
            else
                _currentPageVm = MainPageViewModel;
        }

        private void OnWelcomeFinished()
        {
            SettingsController.Current.WelcomeShown = true;
            SettingsController.Save();
            CurrentPageVm = MainPageViewModel;
        }

        private void OnSettings() {
            CurrentPageVm = SettingsPageViewModel;
        }
        public void ShowFilesPage() {
            CurrentPageVm = MainPageViewModel;
        }
    }

    public class MainPageViewModel : ViewModelBase
    {
        private readonly Action _openSettings;
        public ObservableCollection<FileModel> Files { get; set; } = new();
        public string CurrentPath { get; set; } = "/";
        public ICommand DownloadCommand { get; }
        public ICommand UploadCommand { get; }
        public ICommand MoveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SettingsCommand { get; }

        public MainPageViewModel(Action openSettings)
        {
            DownloadCommand = new RelayCommand(OnDownload);
            UploadCommand = new RelayCommand(OnUpload);
            MoveCommand = new RelayCommand(OnMove);
            DeleteCommand = new RelayCommand(OnDelete);
            SettingsCommand = new RelayCommand(OnSettings);
            Files.Add(new FileModel { Name = "example.txt", VirtualPath = "/", Size = 12345, Hash = "abc123", IsDeleted = false });
            _openSettings = openSettings;
        }
        private void OnDownload() { }
        private void OnUpload() { }
        private void OnMove() { }
        private void OnDelete() { }
        private void OnSettings() { _openSettings?.Invoke(); }
    }
}

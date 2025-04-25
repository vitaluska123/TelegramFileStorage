using System.Collections.ObjectModel;
using TelegramFileStorage.Models;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace TelegramFileStorage.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<FileModel> Files { get; set; } = new();
        public string CurrentPath { get; set; } = "/";

        public ICommand DownloadCommand { get; }
        public ICommand UploadCommand { get; }
        public ICommand MoveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SettingsCommand { get; }

        public MainWindowViewModel()
        {
            DownloadCommand = new RelayCommand(OnDownload);
            UploadCommand = new RelayCommand(OnUpload);
            MoveCommand = new RelayCommand(OnMove);
            DeleteCommand = new RelayCommand(OnDelete);
            SettingsCommand = new RelayCommand(OnSettings);
        }

        private void OnDownload() { /* TODO: Реализация скачивания */ }
        private void OnUpload() { /* TODO: Реализация загрузки */ }
        private void OnMove() { /* TODO: Реализация перемещения */ }
        private void OnDelete() { /* TODO: Реализация удаления */ }
        private void OnSettings() { /* TODO: Открытие настроек */ }
    }
}

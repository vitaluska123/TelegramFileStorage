using System.Collections.ObjectModel;
using TelegramFileStorage.Models;

namespace TelegramFileStorage.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<FileModel> Files { get; set; } = new();
        public string CurrentPath { get; set; } = "/";
        // ...добавить команды для скачивания, загрузки, перемещения, удаления, настроек
    }
}

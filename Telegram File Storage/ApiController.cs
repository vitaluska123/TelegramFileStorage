using System.Threading.Tasks;

namespace TelegramFileStorage
{
    public static class ApiController
    {
        public static async Task<bool> AuthorizeAsync(string login, string password)
        {
            // TODO: Реальная авторизация через сервер
            await Task.Delay(500); // имитация запроса
            return login == "admin" && password == "admin"; // для теста
        }
        // Здесь можно добавить другие методы API (регистрация, проверка токена и т.д.)
    }
}

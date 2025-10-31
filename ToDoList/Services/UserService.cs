using System.Text.Json;
using ToDoList.Models;
using ToDoList.Models.ViewModels;

namespace ToDoList.Services
{
    public class UserService
    {
        private readonly string _filePath;

        public UserService(IConfiguration configuration)
        {
            _filePath = configuration["FileSettings:UserDataPath"];
        }
        private List<User> GetAllUsers()
        {
            string json = File.ReadAllText(_filePath);
            List<User> users = JsonSerializer.Deserialize<List<User>>(json);

            return users ?? new List<User>();
        }

        private void SaveUserToFile(User user)
        {
            var users = GetAllUsers();
            users.Add(user);

            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        private int GenerateUserId(List<User> users)
        {
            if (users == null || users.Count == 0)
                return 1;

            return users.Max(u => u.Id) + 1;
        }

        public User? Authenticate(LoginViewModel model)
        {
            var users = GetAllUsers();
            return users.FirstOrDefault(u =>
            u.Username == model.Username &&
            u.Password == model.Password);
        }

        public bool CreateUser(RegisterViewModel model)
        {
            var users = GetAllUsers();
            bool userExists = users.Any(u => u.Username == model?.Username);

            if (userExists)
            {
                return false;
            }
            else if (model.Password != model.ConfirmPassword) 
            {
                return false;
            }
            else
            {
                var user = new User(
                    GenerateUserId(users),
                    model.Username,
                    model.Password);
                SaveUserToFile(user);
            }
            return true;
        }
    }
}
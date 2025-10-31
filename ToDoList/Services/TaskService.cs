using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Models.ViewModels;

namespace ToDoList.Services
{
    public class TaskService
    {
        private readonly SessionService _sessionService; // dependency injection
        private readonly string _filePath;

        public TaskService(SessionService sessionService,IConfiguration configuration)
        {
            this._sessionService = sessionService;
            _filePath = configuration["FileSettings:UserDataPath"];
        }

        public List<UserTask> GetAllTasks(string username)
        {
            string json = File.ReadAllText(_filePath); // Json Dosyasını Okuduk
            var users = JsonSerializer.Deserialize<List<User>>(json); // Jsonu kullanıcı listesine dönüştürdük
            var user = users?.FirstOrDefault(u => u.Username == username); // Ilgili kullanıcıyı bulduk
            return user?.Tasks ?? new List<UserTask>(); // Bu kullanıcya ait görevleri döndürdük
        }

        public UserTask GetTaskById(int id) 
        {
            var user = _sessionService.GetSession();
            var tasks = GetAllTasks(user);

            var task = tasks?.FirstOrDefault(t => t.Id == id);

            return task;
        }
        private int GenerateTaskId(List<UserTask> tasks) 
        {
            if (tasks == null || tasks.Count == 0)
                { return 1; }
            return tasks.Max(t => t.Id) + 1;
        }

        public void SaveTask(UserTask task)
        {
            string username = _sessionService.GetSession();
            if (task == null) return;

            string json = File.ReadAllText(_filePath);
            var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

            var user = users.FirstOrDefault(u => u.Username == username);
            if (user == null) return;

            if (user.Tasks == null)
                user.Tasks = new List<UserTask>();

            user.Tasks.Add(task);

            string updatedJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, updatedJson);
        }


        public void CreateTask(TaskViewModel model)
        {
            string username = _sessionService.GetSession();
            if (string.IsNullOrEmpty(username)) return;

            var tasks = GetAllTasks(username);

            var task = new UserTask(GenerateTaskId(tasks),
                                    model.Title,
                                    model.Description);
            SaveTask(task);
        }

        public void DeleteTask(int taskId)
        {
            string username = _sessionService.GetSession();
            if (string.IsNullOrEmpty(username)) return;

            string json = File.ReadAllText(_filePath);
            var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

            var user = users.FirstOrDefault(u => u.Username == username);
            if (user == null || user.Tasks == null) return;

            var taskToRemove = user.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToRemove != null)
            {
                user.Tasks.Remove(taskToRemove);
            }

            string updatedJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, updatedJson);
        }

        public void EditTask(int taskId, string newDescription) 
        {
            string username = _sessionService.GetSession();
            if (string.IsNullOrEmpty(username)) return;

            string json = File.ReadAllText(_filePath);
            var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

            var user = users.FirstOrDefault(u => u.Username == username);
            if (user == null || user.Tasks == null) return;

            var taskToEdit = user.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToEdit != null)
            {
                taskToEdit.Description = newDescription;
            }

            string updatedJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, updatedJson);
        }
    }
}

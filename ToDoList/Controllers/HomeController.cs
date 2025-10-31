using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Models.ViewModels;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly SessionService _sessionService;
        private readonly TaskService _taskService;

        public HomeController(SessionService sessionService,TaskService taskService)
        {
            _sessionService = sessionService;
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string? user = _sessionService.GetSession();
            List<UserTask> tasks = _taskService.GetAllTasks(user);

            ViewData["user"] = user;
            ViewData["tasks"] = tasks;
            return View();
        }

        [HttpGet]
        public IActionResult CreateTask() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTask(TaskViewModel model) 
        {
            _taskService.CreateTask(model);
            TempData["Message"] = "✅ Görev başarıyla oluşturuldu.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTask(int id) 
        {
            _taskService.DeleteTask(id);
            TempData["Message"] = "🗑️ Görev başarıyla silindi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditTask(int id) 
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTask(int id, string description) 
        {
            _taskService.EditTask(id, description);
            TempData["Message"] = "✏️ Görev başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
    }
}

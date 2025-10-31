using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.ViewModels;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserService _userService;
        private readonly SessionService _sessionService;

        public LoginController(UserService userService, SessionService sessionService) 
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // (Cross-Site Request Forgery) saldırılarına karşı korur.
        public IActionResult Login(LoginViewModel model)
        {
            var user = _userService.Authenticate(model);

            if (user != null) 
            {
                _sessionService.SetSession(model.Username);
                return RedirectToAction("Index","Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Post - Put - Delete metodlarında kullanılır
        public IActionResult Register(RegisterViewModel model) 
        {
            _userService.CreateUser(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Sunucu bu token’ı doğrular. Böylece sadece kendi uygulamam üzerinden gelen istekleri kabul eder.
        public IActionResult Logout() 
        {
            _sessionService.DeleteSession();
            return RedirectToAction("Login");
        }
    }
}

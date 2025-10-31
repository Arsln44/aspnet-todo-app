using Microsoft.AspNetCore.Http;

namespace ToDoList.Services
{
    public class SessionService
    {
        // ↓ Controller dışında session'a ulaşmak için HttpContext'i sağlar
        private readonly IHttpContextAccessor _contextAccessor;

        // ↓ Session'a veri yazarken kullanılacak sabit anahtardır (Key)
        private const string SessionKey = "Username";

        public SessionService(IHttpContextAccessor contextAccessor) 
        {
            _contextAccessor = contextAccessor; // Dependency Injection
        }

        public void SetSession(string session)
        {
            _contextAccessor.HttpContext.Session.SetString(SessionKey, session);
        }

        public string? GetSession() 
        {
           return _contextAccessor.HttpContext.Session.GetString(SessionKey);
        }

        public void DeleteSession() 
        {
            _contextAccessor.HttpContext.Session.Clear();
        }
    }
}

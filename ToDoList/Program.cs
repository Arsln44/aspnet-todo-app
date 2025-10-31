using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddSession(option => 
{
    option.IdleTimeout = TimeSpan.FromMinutes(30); // → 30 dakikalık oturum süresi
    option.Cookie.HttpOnly = true; // Sadece HTTP protokolü üzerinden erişilir  XSS saldırsını önelemk için
    option.Cookie.IsEssential = true; // Bu çerezin zorunlu olduğunu belirtir  analiz veya reklam amaçlı çerezler genelde non-essential’dir.

});

builder.Services.AddDistributedMemoryCache(); // Session verilerini sunucunun RAM’inde geçici olarak saklamak için

// ↓ dependency Injection
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();


// ↓ Middleware Pipeline Sırası önemli

app.UseStaticFiles(); // wwwroot klasöründeki statik dosyaları (css, js, görseller) sunar
app.UseRouting(); // URL'yi çözümleyip hangi controller/action çalışacak belirler
app.UseSession(); // HTTP oturum yönetimini (Session) etkinleştirir
app.UseAuthorization(); // Yetki (Authorization) kontrolünü devreye alır

app.MapControllerRoute( 
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}"
    ); // Varsayılan route: LoginController > Login action (isteğe bağlı id parametresi)

app.Run(); // Uygulamayı başlatır, HTTP isteklerini almaya başlar

using Microsoft.EntityFrameworkCore;
using PAWScrum.Architecture.Interfaces;
using PAWScrum.Architecture.Providers;
using PAWScrum.Data.Context; // Asegúrate de importar el namespace del DbContext

var builder = WebApplication.CreateBuilder(args);

// Registro de DbContext con cadena de conexión desde appsettings.json
builder.Services.AddDbContext<PAWScrumDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de otros servicios
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IRestProvider, RestProvider>();

builder.Services.AddAuthentication("PAWScrumAuth")
    .AddCookie("PAWScrumAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Welcome}/{action=Index}/{id?}");

app.Run();

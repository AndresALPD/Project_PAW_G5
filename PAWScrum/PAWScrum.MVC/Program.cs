using Microsoft.EntityFrameworkCore;
using PAWScrum.Architecture.Interfaces;
using PAWScrum.Architecture.Providers;
using PAWScrum.Business.Interfaces;
using PAWScrum.Business.Managers;
using PAWScrum.Business.Services;
using PAWScrum.Data.Context;
using PAWScrum.Repositories;
using PAWScrum.Repositories.Implementations;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Services.Interfaces;
using PAWScrum.Services.Service;

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

builder.Services.AddScoped<ISprintService, SprintService>();
builder.Services.AddScoped<ISprintBusiness, SprintBusiness>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProductBacklogBusiness, ProductBacklogBusiness>();
builder.Services.AddScoped<IProductBacklogService, ProductBacklogService>();
builder.Services.AddScoped<IProductBacklogRepository, ProductBacklogRepository>();
builder.Services.AddScoped<ISprintBacklogService, SprintBacklogService>();
builder.Services.AddScoped<ISprintBacklogRepository, SprintBacklogRepository>();
builder.Services.AddScoped<ISprintBacklogBusiness, SprintBacklogBusiness>();

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

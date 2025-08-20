using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PAWScrum.Architecture.Interfaces;
using PAWScrum.Architecture.Providers;
using PAWScrum.Business.Interfaces;
using PAWScrum.Business.Managers;
using PAWScrum.Business.Services;
using PAWScrum.Data.Context;
using PAWScrum.Repositories;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Repositories.Implementations;
using PAWScrum.Services.Interfaces;
using PAWScrum.Services.Service;
using PAWScrum.Services;

var builder = WebApplication.CreateBuilder(args);

// Program.cs (para .NET 6 en adelante)
var apiUrl = Environment.GetEnvironmentVariable("ApiUrl");

// Configurar HttpClient para RestProvider
builder.Services.AddHttpClient<IRestProvider, RestProvider>(client =>
{
    client.BaseAddress = new Uri(apiUrl ?? "http://localhost:7058"); // fallback opcional
});

// Configurar controladores con opciones JSON
builder.Services.AddControllersWithViews()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Configurar DbContext
builder.Services.AddDbContext<PAWScrumDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// Configurar autenticación por cookies
builder.Services.AddAuthentication("PAWScrumAuth")
    .AddCookie("PAWScrumAuth", o =>
    {
        o.LoginPath = "/Account/Login";
        o.LogoutPath = "/Account/Logout";
        o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        o.SlidingExpiration = true;
    });

// Registrar servicios HttpClient
builder.Services.AddHttpClient<ISprintService, SprintService>();
builder.Services.AddHttpClient<IProductBacklogService, ProductBacklogService>();

// Registrar repositorios
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();
builder.Services.AddScoped<IProductBacklogRepository, ProductBacklogRepository>();
builder.Services.AddScoped<ISprintBacklogRepository, SprintBacklogRepository>();

// Registrar servicios de negocio
builder.Services.AddScoped<IProductBacklogBusiness, ProductBacklogBusiness>();
builder.Services.AddScoped<ISprintBusiness, SprintBusiness>();
builder.Services.AddScoped<ISprintBacklogBusiness, SprintBacklogBusiness>();

// Registrar servicios de aplicación
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISprintService, SprintService>();
builder.Services.AddScoped<IProductBacklogService, ProductBacklogService>();
builder.Services.AddScoped<ISprintBacklogService, SprintBacklogService>();

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
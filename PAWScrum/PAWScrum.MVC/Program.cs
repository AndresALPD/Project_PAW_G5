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
using PAWScrum.Services;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Configuración de HttpClient para RestProvider
var apiUrl = Environment.GetEnvironmentVariable("ApiUrl");
builder.Services.AddHttpClient<IRestProvider, RestProvider>(client =>
{
    client.BaseAddress = new Uri(apiUrl ?? "http://localhost:7058"); // fallback opcional
});

// Configuración de controllers y JSON
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddControllersWithViews();

// Configuración de DbContext
builder.Services.AddDbContext<PAWScrumDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper (de BranchPao)
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// Autenticación
builder.Services.AddAuthentication("PAWScrumAuth")
    .AddCookie("PAWScrumAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// ===== REGISTRO DE SERVICIOS =====

// Servicios de QA (gestión de proyectos y sprints)
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISprintService, SprintService>();
builder.Services.AddScoped<ISprintBusiness, SprintBusiness>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();
builder.Services.AddScoped<IProductBacklogBusiness, ProductBacklogBusiness>();
builder.Services.AddScoped<IProductBacklogService, ProductBacklogService>();
builder.Services.AddScoped<IProductBacklogRepository, ProductBacklogRepository>();
builder.Services.AddScoped<ISprintBacklogService, SprintBacklogService>();
builder.Services.AddScoped<ISprintBacklogRepository, SprintBacklogRepository>();
builder.Services.AddScoped<ISprintBacklogBusiness, SprintBacklogBusiness>();

// HttpClient services de QA
builder.Services.AddHttpClient<ISprintService, SprintService>();
builder.Services.AddHttpClient<IProductBacklogService, ProductBacklogService>();

// Servicios de BranchPao (nuevos módulos)
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

var app = builder.Build();

// Configuración del pipeline HTTP
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
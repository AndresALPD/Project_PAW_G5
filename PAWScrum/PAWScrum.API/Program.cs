using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PAWScrum.Business.Interfaces;
using PAWScrum.Business.Managers;
using PAWScrum.Data.Context;
using PAWScrum.Repositories.Implementations;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Services;
using PAWScrum.Services.Interfaces;
using PAWScrum.Services.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<PAWScrumDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>(); 
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PAWScrum API",
        Version = "v1"
    });
});
var cs = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"[DB] Using connection string: {cs}");
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PAWScrum API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

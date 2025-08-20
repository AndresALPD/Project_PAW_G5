using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PAWScrum.Business;
using PAWScrum.Business.Interfaces;
using PAWScrum.Business.Managers;
using PAWScrum.Data.Context;
using PAWScrum.Repositories;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Repositories.Implementations;
using PAWScrum.Services.Interfaces;
using PAWScrum.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using PAWScrum.Services;

var builder = WebApplication.CreateBuilder(args);

// Obtener cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configurar DbContext con logging y características de desarrollo
builder.Services.AddDbContext<PAWScrumDbContext>(options =>
    options.UseSqlServer(connectionString)
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));

// Configurar controladores con manejo de referencias circulares
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configuración de autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateLifetime = true
        };
    });

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Swagger con soporte para JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PAWScrum API", Version = "v1" });

    // Habilitar autorización en Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Ejemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Repositorios
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();

// Servicios de negocio
builder.Services.AddScoped<IProjectBusiness, ProjectBusiness>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
builder.Services.AddScoped<ISprintBusiness, SprintBusiness>();

// Servicios de aplicación
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();

// CORS para desarrollo
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PAWScrum API V1");
        options.RoutePrefix = "swagger";
        options.OAuthClientId("swagger-ui");
        options.OAuthAppName("Swagger UI");
    });
}

// Middleware para capturar errores
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error no manejado: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
        throw;
    }
});

// Aplicar migraciones automáticamente en desarrollo
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<PAWScrumDbContext>();
        try
        {
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al aplicar migraciones: {ex.Message}");
        }
    }
}

app.UseCors("DevCorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
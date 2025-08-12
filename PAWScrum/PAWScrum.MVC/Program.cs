using AutoMapper;
using PAWScrum.Architecture.Interfaces;
using PAWScrum.Architecture.Providers;
using PAWScrum.Repositories.Implementations;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Services;
using PAWScrum.Services.Interfaces;
using PAWScrum.Services.Service;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IRestProvider, RestProvider>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddHttpClient();

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


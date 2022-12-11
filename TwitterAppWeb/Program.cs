using Microsoft.EntityFrameworkCore;
using Serilog;
using TwitterAppWeb.Hubs;
using TwitterAppWeb.Interfaces;
using TwitterAppWeb.Repositories;
using TwitterAppWeb.Services;
using TwitterAppWeb.Workers;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/TwitterApp-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Sql services
builder.Services.AddEntityFrameworkSqlite();
builder.Services.AddDbContext<TwitterContext>();

// Services
builder.Services.AddScoped<ITwitterAnalyticService, TwitterAnalyticService>();
builder.Services.AddScoped<ITwitterConsumerService, TwitterConsumerService>();
builder.Services.AddSingleton<ISerializationService, SerializationService>();
builder.Services.AddScoped<ITwitterService, TwitterService>();
            
// Repositories
builder.Services.AddScoped<ITwitterRepository, TwitterRepository>();
builder.Services.AddScoped<ITwitterAnalyticRepository, TwitterAnalyticRepository>();

builder.Services.AddHostedService<GetTweetWorker>();

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<TwitterContext>();
    dataContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<TwitterHub>("/twitterHub");

app.Run();
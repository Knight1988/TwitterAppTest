using Microsoft.EntityFrameworkCore;
using Serilog;
using TwitterApp;
using TwitterApp.Interfaces;
using TwitterApp.Repositories;
using TwitterApp.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/TwitterApp.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddEntityFrameworkSqlite().AddDbContext<TwitterContext>(ServiceLifetime.Singleton);
        
        services.AddSingleton<ITwitterService, TwitterService>();
        services.AddSingleton<ISerializationService, SerializationService>();
        
        services.AddSingleton<ITwitterRepository, TwitterRepository>();
        
        services.AddHostedService<GetTweetWorker>();
    })
    .UseSerilog()
    .Build();

// migrate any database changes on startup (includes initial db creation)
var twitterContext = host.Services.GetRequiredService<TwitterContext>();
await twitterContext.Database.MigrateAsync();

await host.RunAsync();
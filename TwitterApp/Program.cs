using Serilog;
using TwitterApp;
using TwitterApp.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/TwitterApp.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ISerializationService, SerializationService>();
        services.AddHostedService<GetTweetWorker>();
    })
    .Build();

await host.RunAsync();
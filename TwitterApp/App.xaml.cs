using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TwitterApp.Interfaces;
using TwitterApp.Repositories;
using TwitterApp.Services;
using TwitterApp.ViewModels;

namespace TwitterApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost host;

        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/TwitterApp-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            host = Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .UseSerilog()
                .Build();

            ServiceProvider = host.Services;
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var context = host.Services.GetService<TwitterContext>();
            context.Database.Migrate();
            
            var mainWindow = host.Services.GetService<MainWindow>();
            mainWindow.Show();

            host.RunAsync();
        }

        private void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            // Sql services
            services.AddEntityFrameworkSqlite();
            services.AddDbContext<TwitterContext>(ServiceLifetime.Singleton);

            // Services
            services.AddSingleton<ITwitterAnalyticService, TwitterAnalyticService>();
            services.AddSingleton<ITwitterConsumerService, TwitterConsumerService>();
            services.AddSingleton<ISerializationService, SerializationService>();
            services.AddSingleton<ITwitterService, TwitterService>();
            
            // Repositories
            services.AddSingleton<ITwitterRepository, TwitterRepository>();
            services.AddSingleton<ITwitterAnalyticRepository, TwitterAnalyticRepository>();

            // Windows
            services.AddSingleton<MainWindow>();

            // View Models
            services.AddSingleton<MainViewModel>();
            
            // Worker
            services.AddSingleton<GetTweetBackgroundWorker>();
            services.AddSingleton<TweetAnalyticWorker>();
        }
    }
}
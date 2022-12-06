using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TwitterApp.Interfaces;
using TwitterApp.Repositories;
using TwitterApp.Services;

namespace TwitterApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost host;

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
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
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
            services.AddSingleton<ISerializationService, SerializationService>();
            services.AddSingleton<ITwitterService, TwitterService>();
            
            // Repositories
            services.AddSingleton<ITwitterRepository, TwitterRepository>();

            // Windows
            services.AddSingleton<MainWindow>();
        }
    }
}
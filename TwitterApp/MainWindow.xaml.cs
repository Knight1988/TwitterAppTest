using System.ComponentModel;
using System.Windows;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using TwitterApp.Repositories;

namespace TwitterApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MainWindow(GetTweetBackgroundWorker getTweetBackgroundWorker, 
            TweetAnalyticBackgroundWorker tweetAnalyticBackgroundWorker,
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            InitializeComponent();
            
            getTweetBackgroundWorker.Error += GetTweetBackgroundWorkerOnError;
            tweetAnalyticBackgroundWorker.Error += TweetAnalyticBackgroundWorkerOnError;
            
            getTweetBackgroundWorker.RunWorkerAsync();
            tweetAnalyticBackgroundWorker.RunWorkerAsync();
        }

        private void TweetAnalyticBackgroundWorkerOnError(object? sender, string e)
        {
            MessageBox.Show("Some error occurred, please send log file to administrator", "Analytic Tweet Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

        private void GetTweetBackgroundWorkerOnError(object? sender, string e)
        {
            MessageBox.Show("Some error occurred, please send log file to administrator", "Get Tweet Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

        private async void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<TwitterContext>();
            // Clean up DB upon exit
            await context.GetInfrastructure().GetService<IMigrator>().MigrateAsync("0");
        }
    }
}
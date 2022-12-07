using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            MessageBox.Show(e, "Analytic Tweet Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void GetTweetBackgroundWorkerOnError(object? sender, string e)
        {
            MessageBox.Show(e, "Get Tweet Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
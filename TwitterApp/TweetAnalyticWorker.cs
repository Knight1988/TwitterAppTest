using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TwitterApp.Interfaces;
using TwitterApp.ViewModels;

namespace TwitterApp;

public class TweetAnalyticWorker : BackgroundWorker
{
    private readonly ILogger<TweetAnalyticWorker> _logger;
    private readonly MainViewModel _mainViewModel;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TweetAnalyticWorker(ILogger<TweetAnalyticWorker> logger, MainViewModel mainViewModel, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _mainViewModel = mainViewModel;
        _serviceScopeFactory = serviceScopeFactory;
        DoWork += OnDoWork;
    }
    
    private async void OnDoWork(object? sender, DoWorkEventArgs e)
    {
        try
        {
            await AnalyticTweetAsync(e);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "There was error when analytic tweet");
        }
    }

    private async Task AnalyticTweetAsync(DoWorkEventArgs e)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var twitterAnalyticService = scope.ServiceProvider.GetService<ITwitterAnalyticService>();
        
        while (!e.Cancel)
        {
            _mainViewModel.TweetCount = await twitterAnalyticService.GetTotalTweetCountAsync();
            _mainViewModel.AverageTweetPerMinute = await twitterAnalyticService.GetAverageTweetsPerMinuteAsync();
            await Task.Delay(1000);
        }
    }
}
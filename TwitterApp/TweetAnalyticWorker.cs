using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TwitterApp.Interfaces;
using TwitterApp.ViewModels;

namespace TwitterApp;

public class TweetAnalyticWorker : BackgroundWorker
{
    private readonly ILogger<TweetAnalyticWorker> _logger;
    private readonly ITwitterAnalyticService _twitterAnalyticService;
    private readonly MainViewModel _mainViewModel;

    public TweetAnalyticWorker(ILogger<TweetAnalyticWorker> logger, ITwitterAnalyticService twitterAnalyticService, 
        MainViewModel mainViewModel)
    {
        _logger = logger;
        _twitterAnalyticService = twitterAnalyticService;
        _mainViewModel = mainViewModel;
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
        while (!e.Cancel)
        {
            _mainViewModel.TweetCount = await _twitterAnalyticService.GetTotalTweetCountAsync();
            _mainViewModel.AverageTweetPerMinute = await _twitterAnalyticService.GetAverageTweetsPerMinuteAsync();
            await Task.Delay(1000);
        }
    }
}
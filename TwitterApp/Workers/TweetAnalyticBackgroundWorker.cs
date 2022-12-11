using System;
using System.ComponentModel;
using System.Threading.Tasks;
using LiveCharts.Defaults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TwitterApp.Interfaces;
using TwitterApp.ViewModels;

namespace TwitterApp.Workers;

public class TweetAnalyticBackgroundWorker : BackgroundWorker
{
    private readonly ILogger<TweetAnalyticBackgroundWorker> _logger;
    private readonly MainViewModel _mainViewModel;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TweetAnalyticBackgroundWorker(ILogger<TweetAnalyticBackgroundWorker> logger, MainViewModel mainViewModel, IServiceScopeFactory serviceScopeFactory)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "There was error when analytic tweet");
            OnError(ex.Message);
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
            
            // update average tweet chart
            _mainViewModel.AverageTweetObservableValues.Add(new ObservableValue(_mainViewModel.AverageTweetPerMinute));
            if (_mainViewModel.AverageTweetObservableValues.Count > 10)
            {
                _mainViewModel.AverageTweetObservableValues.RemoveAt(0);
            }
            
            // update total tweet chart
            _mainViewModel.TweetReceivedObservableValues.Add(new ObservableValue(_mainViewModel.TweetCount));
            if (_mainViewModel.TweetReceivedObservableValues.Count > 10)
            {
                _mainViewModel.TweetReceivedObservableValues.RemoveAt(0);
            }
            await Task.Delay(1000);
        }
    }

    public event EventHandler<string> Error;

    protected virtual void OnError(string e)
    {
        Error?.Invoke(this, e);
    }
}
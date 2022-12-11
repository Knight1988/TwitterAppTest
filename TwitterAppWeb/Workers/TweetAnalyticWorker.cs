using Microsoft.AspNetCore.SignalR;
using TwitterApp.Core.Interfaces;
using TwitterAppWeb.Hubs;

namespace TwitterAppWeb.Workers;

public class TweetAnalyticWorker : BackgroundService
{
    private readonly ILogger<TweetAnalyticWorker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHubContext<TwitterHub, ITwitterHub> _twitterHub;

    public TweetAnalyticWorker(ILogger<TweetAnalyticWorker> logger, IServiceScopeFactory serviceScopeFactory,
        IHubContext<TwitterHub, ITwitterHub> twitterHub)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _twitterHub = twitterHub;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await AnalyticTweetAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "There was error when analytic tweet");
        }
    }
    
    public async Task AnalyticTweetAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var twitterAnalyticService = scope.ServiceProvider.GetService<ITwitterAnalyticService>();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var tweetCount = await twitterAnalyticService.GetTotalTweetCountAsync();
            var averageTweetPerMinute = await twitterAnalyticService.GetAverageTweetsPerMinuteAsync();

            await _twitterHub.Clients.All.ReceiveAnalytic(tweetCount, averageTweetPerMinute);
            await Task.Delay(1000);
        }
    }
}
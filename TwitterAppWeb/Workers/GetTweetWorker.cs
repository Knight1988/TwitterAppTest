using System.Text;
using Microsoft.AspNetCore.SignalR;
using TwitterApp.Core.Interfaces;
using TwitterAppWeb.Hubs;

namespace TwitterAppWeb.Workers;

public class GetTweetWorker : BackgroundService
{
    private readonly ILogger<GetTweetWorker> _logger;
    private readonly ISerializationService _serializationService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHubContext<TwitterHub,ITwitterHub> _twitterHub;

    public GetTweetWorker(ILogger<GetTweetWorker> logger, ISerializationService serializationService, 
        IServiceScopeFactory serviceScopeFactory, IHubContext<TwitterHub, ITwitterHub> twitterHub)
    {
        _logger = logger;
        _serializationService = serializationService;
        _serviceScopeFactory = serviceScopeFactory;
        _twitterHub = twitterHub;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await GetSampleStreamAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was error when get sample stream");
                await _twitterHub.Clients.All.ReceiveError(ex.Message);
            }

            await Task.Delay(1000);
        }
    }
    
    public async Task GetSampleStreamAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var twitterConsumerService = scope.ServiceProvider.GetService<ITwitterConsumerService>();
        var twitterService = scope.ServiceProvider.GetService<ITwitterService>();

        var stream = await twitterConsumerService.GetSampleStreamAsync();
        var json = string.Empty;
        
        var buffer = new byte[1024];
        while (!stoppingToken.IsCancellationRequested)
        {
            if (stream.CanRead)
            {
                // get json string
                var length = await stream.ReadAsync(buffer, 0, buffer.Length);
                json += Encoding.UTF8.GetString(buffer, 0, length).Trim();
                // convert to model
                var tweetModels = _serializationService.Deserialize(ref json);
                // save to db
                await twitterService.SaveDataAsync(tweetModels);
            }

            // await Task.Delay(1000);
        }
    }
}
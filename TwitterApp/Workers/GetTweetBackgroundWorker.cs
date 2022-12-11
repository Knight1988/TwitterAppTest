using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TwitterApp.Interfaces;

namespace TwitterApp.Workers;

public class GetTweetBackgroundWorker : BackgroundWorker
{
    private readonly ILogger<GetTweetBackgroundWorker> _logger;
    private readonly ISerializationService _serializationService;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GetTweetBackgroundWorker(ILogger<GetTweetBackgroundWorker> logger, ISerializationService serializationService, 
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serializationService = serializationService;
        _serviceScopeFactory = serviceScopeFactory;
        DoWork += OnDoWork;
    }

    private async void OnDoWork(object? sender, DoWorkEventArgs e)
    {
        try
        {
            await GetSampleStreamAsync(e);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "There was error when get sample stream");
            OnError(ex.Message);
        }
    }

    private async Task GetSampleStreamAsync(DoWorkEventArgs e)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var twitterConsumerService = scope.ServiceProvider.GetService<ITwitterConsumerService>();
        var twitterService = scope.ServiceProvider.GetService<ITwitterService>();

        var stream = await twitterConsumerService.GetSampleStreamAsync();
        var json = string.Empty;
        while (!e.Cancel)
        {
            if (stream.CanRead)
            {
                // get json string
                var buffer = new byte[1024];
                var length = await stream.ReadAsync(buffer, 0, buffer.Length);
                json += Encoding.UTF8.GetString(buffer, 0, length).Trim();
                _logger.LogInformation("API Response: {Json}", json);
                // convert to model
                var tweetModels = _serializationService.Deserialize(ref json);
                // save to db
                if (tweetModels != null)
                {
                    await twitterService.SaveDataAsync(tweetModels);
                }
                else
                {
                    _logger.LogWarning("");
                }
            }

            // await Task.Delay(1000);
        }
    }

    public event EventHandler<string> Error;

    protected virtual void OnError(string e)
    {
        Error?.Invoke(this, e);
    }
}
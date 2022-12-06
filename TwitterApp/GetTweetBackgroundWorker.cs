using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TwitterApp.Interfaces;

namespace TwitterApp;

public class GetTweetBackgroundWorker : BackgroundWorker
{
    private readonly ILogger<GetTweetBackgroundWorker> _logger;
    private readonly ISerializationService _serializationService;
    private readonly ITwitterService _twitterService;
    private readonly ITwitterConsumerService _twitterConsumerService;

    public GetTweetBackgroundWorker(ILogger<GetTweetBackgroundWorker> logger, ISerializationService serializationService, 
        ITwitterService twitterService, ITwitterConsumerService twitterConsumerService)
    {
        _logger = logger;
        _serializationService = serializationService;
        _twitterService = twitterService;
        _twitterConsumerService = twitterConsumerService;
        DoWork += OnDoWork;
    }

    private async void OnDoWork(object? sender, DoWorkEventArgs e)
    {
        try
        {
            await GetSampleStreamAsync(e);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "There was error when get sample stream");
        }
    }

    private async Task GetSampleStreamAsync(DoWorkEventArgs e)
    {
        var stream = await _twitterConsumerService.GetSampleStreamAsync();
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
                await _twitterService.SaveDataAsync(tweetModels);
            }

            // await Task.Delay(1000);
        }
    }
}
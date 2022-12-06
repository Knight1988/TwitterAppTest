using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwitterApp.Interfaces;
using TwitterApp.Services;

namespace TwitterApp;

public class GetTweetBackgroundWorker : BackgroundWorker
{
    private readonly ILogger<GetTweetBackgroundWorker> _logger;
    private readonly ISerializationService _serializationService;
    private readonly IConfiguration _configuration;
    private readonly ITwitterService _twitterService;
    private readonly ITwitterConsumerService _twitterConsumerService;

    public GetTweetBackgroundWorker(ILogger<GetTweetBackgroundWorker> logger, IConfiguration configuration, 
        ISerializationService serializationService, ITwitterService twitterService, 
        ITwitterConsumerService twitterConsumerService)
    {
        _logger = logger;
        _configuration = configuration;
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
            Console.WriteLine(exception);
            throw;
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

            await Task.Delay(1000);
        }
    }
}
using System.Net.Http.Headers;
using System.Text;
using TwitterApp.Services;

namespace TwitterApp;

public class GetTweetWorker : BackgroundService
{
    private readonly ILogger<GetTweetWorker> _logger;
    private readonly ISerializationService _serializationService;
    private readonly IConfiguration _configuration;

    public GetTweetWorker(ILogger<GetTweetWorker> logger, IConfiguration configuration, ISerializationService serializationService)
    {
        _logger = logger;
        _configuration = configuration;
        _serializationService = serializationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(_configuration.GetValue<string>("Twitter:Auth"));
        var stream = await client.GetStreamAsync("https://api.twitter.com/2/tweets/sample/stream");
        var json = string.Empty;
        while (!stoppingToken.IsCancellationRequested)
        {
            if (stream.CanRead)
            {
                // get json string
                var buffer = new byte[1024];
                var length = await stream.ReadAsync(buffer, 0, buffer.Length);
                json += Encoding.UTF8.GetString(buffer, 0, length).Trim();
                // convert to model
                _serializationService.Deserialize(ref json);
                _logger.LogInformation("API Response: {Json}", json);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
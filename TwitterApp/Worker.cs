using System.Net.Http.Headers;
using System.Text;

namespace TwitterApp;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer AAAAAAAAAAAAAAAAAAAAAPlMkAEAAAAANYDpDca0sEuDjiZrQNCHMq4OvHI%3D97OsE4047R4bbCq4Vyt1qvex1boRKw3oDQhuMuyELvn0z9YCJO");
        var stream = await client.GetStreamAsync("https://api.twitter.com/2/tweets/sample/stream");
        while (!stoppingToken.IsCancellationRequested)
        {
            if (stream.CanRead)
            {
                var b = new byte[1024];
                var length = await stream.ReadAsync(b, 0, b.Length);
                var text = Encoding.UTF8.GetString(b, 0, length).Trim();
            }
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
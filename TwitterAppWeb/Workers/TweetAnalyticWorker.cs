namespace TwitterAppWeb.Workers;

public class TweetWorker : BackgroundService
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
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
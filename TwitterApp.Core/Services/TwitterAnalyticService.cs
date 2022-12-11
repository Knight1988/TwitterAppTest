using TwitterApp.Core.Interfaces;

namespace TwitterApp.Core.Services;

public class TwitterAnalyticService : ITwitterAnalyticService
{
    private readonly ITwitterAnalyticRepository _twitterAnalyticRepository;

    public TwitterAnalyticService(ITwitterAnalyticRepository twitterAnalyticRepository)
    {
        _twitterAnalyticRepository = twitterAnalyticRepository;
    }
    
    public async Task<int> GetTotalTweetCountAsync()
    {
        return await _twitterAnalyticRepository.GetTotalTweetCountAsync();
    }

    public async Task<double> GetAverageTweetsPerMinuteAsync()
    {
        const double totalMinutes = 5;
        var fromDateTime = DateTime.Now.AddMinutes(-totalMinutes);
        var count = await _twitterAnalyticRepository.GetTweetCountFromMinuteAsync(fromDateTime);
        
        // calculate average tweets per minute
        return Convert.ToInt32(Math.Round(count / totalMinutes));
    }
}
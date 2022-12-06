using System.Threading.Tasks;

namespace TwitterApp.Interfaces;

public interface ITwitterAnalyticService
{
    Task<int> GetTotalTweetCountAsync();
    Task<double> GetAverageTweetsPerMinuteAsync();
}
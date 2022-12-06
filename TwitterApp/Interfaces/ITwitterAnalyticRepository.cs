using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterApp.Models;

namespace TwitterApp.Interfaces;

public interface ITwitterAnalyticRepository
{
    Task<int> GetTotalTweetCountAsync();
    Task<List<TweetModel>> GetLatestTweetsAsync(int sampleCount);
}
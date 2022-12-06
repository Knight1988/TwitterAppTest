using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterApp.Models;

namespace TwitterApp.Interfaces;

public interface ITwitterAnalyticRepository
{
    Task<int> GetTotalTweetCount();
    Task<List<TweetModel>> GetLatestTweets(int sampleCount);
}
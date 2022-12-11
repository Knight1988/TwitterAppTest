using TwitterApp.Core.Models;

namespace TwitterApp.Core.Interfaces;

public interface ITwitterService
{
    Task SaveDataAsync(IEnumerable<TweetModel> tweetModels);
}
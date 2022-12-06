using TwitterApp.Models;

namespace TwitterApp.Interfaces;

public interface ITwitterService
{
    Task SaveDataAsync(List<TweetModel> tweetModels);
}
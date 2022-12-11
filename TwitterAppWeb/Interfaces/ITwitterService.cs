using TwitterAppWeb.Models;

namespace TwitterAppWeb.Interfaces;

public interface ITwitterService
{
    Task SaveDataAsync(IEnumerable<TweetModel> tweetModels);
}
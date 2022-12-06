using TwitterApp.Models;

namespace TwitterApp.Interfaces;

public interface ITwitterRepository
{
    public Task UpsertAsync(IEnumerable<TweetModel> tweetModels);
}
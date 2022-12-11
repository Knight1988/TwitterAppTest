using TwitterApp.Core.Models;

namespace TwitterApp.Core.Interfaces;

public interface ITwitterRepository
{
    public Task<int> UpsertAsync(IEnumerable<TweetModel> tweetModels);
}
using TwitterAppWeb.Models;

namespace TwitterAppWeb.Interfaces;

public interface ITwitterRepository
{
    public Task<int> UpsertAsync(IEnumerable<TweetModel> tweetModels);
}
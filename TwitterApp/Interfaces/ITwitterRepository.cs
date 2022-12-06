using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterApp.Models;

namespace TwitterApp.Interfaces;

public interface ITwitterRepository
{
    public Task<int> UpsertAsync(IEnumerable<TweetModel> tweetModels);
}
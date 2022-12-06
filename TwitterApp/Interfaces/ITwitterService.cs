using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterApp.Models;

namespace TwitterApp.Interfaces;

public interface ITwitterService
{
    Task SaveDataAsync(IEnumerable<TweetModel> tweetModels);
}
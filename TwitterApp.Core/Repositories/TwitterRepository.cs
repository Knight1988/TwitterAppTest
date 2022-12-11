using Microsoft.EntityFrameworkCore;
using TwitterApp.Core.Interfaces;
using TwitterApp.Core.Models;

namespace TwitterApp.Core.Repositories;

public class TwitterRepository : ITwitterRepository
{
    private readonly TwitterContext _context;

    public TwitterRepository(TwitterContext context)
    {
        _context = context;
    }
    
    public async Task<int> UpsertAsync(IEnumerable<TweetModel> tweetModels)
    {
        return await _context.Tweets.UpsertRange(tweetModels)
            .On(p => p.Id)
            .RunAsync();
    }
}
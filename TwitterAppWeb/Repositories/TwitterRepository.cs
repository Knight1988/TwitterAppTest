using Microsoft.EntityFrameworkCore;
using TwitterAppWeb.Interfaces;
using TwitterAppWeb.Models;

namespace TwitterAppWeb.Repositories;

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
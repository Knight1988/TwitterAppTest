using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwitterApp.Interfaces;
using TwitterApp.Models;

namespace TwitterApp.Repositories;

public class TwitterAnalyticRepository : ITwitterAnalyticRepository
{
    private readonly TwitterContext _context;

    public TwitterAnalyticRepository(TwitterContext context)
    {
        _context = context;
    }
    
    public async Task<int> GetTotalTweetCount()
    {
        return await _context.Tweets.CountAsync();
    }
    
    public async Task<List<TweetModel>> GetLatestTweets(int sampleCount)
    {
        return await _context.Tweets.OrderByDescending(t => t.CreatedTime).Take(sampleCount).ToListAsync();
    }
}
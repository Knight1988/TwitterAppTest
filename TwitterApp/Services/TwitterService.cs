using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TwitterApp.Interfaces;
using TwitterApp.Models;
using TwitterApp.Repositories;

namespace TwitterApp.Services;

public class TwitterService : ITwitterService
{
    private readonly ITwitterRepository _twitterRepository;
    private readonly ILogger<TwitterService> _logger;

    public TwitterService(ITwitterRepository twitterRepository, ILogger<TwitterService> logger)
    {
        _logger = logger;
        _twitterRepository = twitterRepository;
    }

    public async Task SaveDataAsync(List<TweetModel> tweetModels)
    {
        await _twitterRepository.UpsertAsync(tweetModels);
        _logger.LogInformation("Updated {Number} tweets", tweetModels.Count);
    }
}
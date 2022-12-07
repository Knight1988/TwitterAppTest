﻿using System;
using System.Linq;
using System.Threading.Tasks;
using TwitterApp.Interfaces;

namespace TwitterApp.Services;

public class TwitterAnalyticService : ITwitterAnalyticService
{
    private readonly ITwitterAnalyticRepository _twitterAnalyticRepository;

    public TwitterAnalyticService(ITwitterAnalyticRepository twitterAnalyticRepository)
    {
        _twitterAnalyticRepository = twitterAnalyticRepository;
    }
    
    public async Task<int> GetTotalTweetCountAsync()
    {
        return await _twitterAnalyticRepository.GetTotalTweetCountAsync();
    }

    public async Task<double> GetAverageTweetsPerMinuteAsync()
    {
        var tweets = await _twitterAnalyticRepository.GetLatestTweetsAsync(1000);
        var oldestTweet = tweets.LastOrDefault();
        // no tweet, return 0
        if (oldestTweet == null) return 0;
        
        // get total minutes
        var totalMinutes = (DateTime.Now - oldestTweet.CreatedTime).TotalMinutes;
        
        // min calculation is 1 minutes
        if (totalMinutes < 1) totalMinutes = 1;
        
        // calculate average tweets per minute
        return Convert.ToInt32(Math.Round(tweets.Count / totalMinutes));
    }
}
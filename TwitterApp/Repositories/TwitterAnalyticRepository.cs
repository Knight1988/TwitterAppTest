﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwitterApp.Interfaces;

namespace TwitterApp.Repositories;

public class TwitterAnalyticRepository : ITwitterAnalyticRepository
{
    private readonly TwitterContext _context;

    public TwitterAnalyticRepository(TwitterContext context)
    {
        _context = context;
    }
    
    public async Task<int> GetTotalTweetCountAsync()
    {
        return await _context.Tweets.CountAsync();
    }

    public async Task<int> GetTweetCountFromMinuteAsync(DateTime fromDateTime)
    {
        return await _context.Tweets.CountAsync(p => p.CreatedTime > fromDateTime);
    }
}
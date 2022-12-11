﻿namespace TwitterAppWeb.Interfaces;

public interface ITwitterAnalyticService
{
    Task<int> GetTotalTweetCountAsync();
    Task<double> GetAverageTweetsPerMinuteAsync();
}
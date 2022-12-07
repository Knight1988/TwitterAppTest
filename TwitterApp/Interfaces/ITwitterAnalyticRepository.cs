using System;
using System.Threading.Tasks;

namespace TwitterApp.Interfaces;

public interface ITwitterAnalyticRepository
{
    Task<int> GetTotalTweetCountAsync();
    Task<int> GetTweetCountFromMinuteAsync(DateTime fromDateTime);
}
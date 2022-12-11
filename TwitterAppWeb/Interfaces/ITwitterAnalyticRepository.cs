namespace TwitterAppWeb.Interfaces;

public interface ITwitterAnalyticRepository
{
    Task<int> GetTotalTweetCountAsync();
    Task<int> GetTweetCountFromMinuteAsync(DateTime fromDateTime);
}
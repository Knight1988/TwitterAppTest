namespace TwitterApp.Core.Interfaces;

public interface ITwitterHub
{
    Task ReceiveAnalytic(int tweetCount, double averageTweetPerMinute);
}
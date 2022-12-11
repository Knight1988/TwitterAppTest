using Microsoft.AspNetCore.SignalR;

namespace TwitterAppWeb.Hubs;

public class TwitterHub : Hub<ITwitterHub>
{
    public async Task SendAnalytic(int tweetCount, double averageTweetPerMinute)
    {
        await Clients.All.ReceiveAnalytic(tweetCount, averageTweetPerMinute);
    }
}

public interface ITwitterHub
{
    Task ReceiveAnalytic(int tweetCount, double averageTweetPerMinute);
}
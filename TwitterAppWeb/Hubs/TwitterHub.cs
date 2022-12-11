using Microsoft.AspNetCore.SignalR;
using TwitterApp.Core.Interfaces;

namespace TwitterAppWeb.Hubs;

public class TwitterHub : Hub<ITwitterHub>
{
    public async Task SendAnalytic(int tweetCount, double averageTweetPerMinute)
    {
        await Clients.All.ReceiveAnalytic(tweetCount, averageTweetPerMinute);
    }
}
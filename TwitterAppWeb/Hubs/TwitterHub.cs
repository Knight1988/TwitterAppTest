using Microsoft.AspNetCore.SignalR;

namespace TwitterAppWeb.Hubs;

public class TwitterHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
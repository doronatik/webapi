using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChatSample.Hubs
{
    public class ChatHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("identifyMessage");
            await base.OnConnectedAsync();
        }

        public async void Identify(string name)
        {
            UserManager.Instance.Dictionary.Add(Context.ConnectionId, name);
            await Clients.All.SendAsync("broadcastMessage", "SYSTEM", $"Welcome - {name}");
        }

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync("broadcastMessage", name, message);
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string username = UserManager.Instance.Dictionary[Context.ConnectionId];
            await Clients.All.SendAsync("broadcastMessage", "SYSTEM", $"{username} left.");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
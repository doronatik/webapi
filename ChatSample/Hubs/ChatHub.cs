using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChatSample.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync("broadcastMessage", name, message);
        }

        public override async Task OnConnectedAsync()
        {

            await Clients.All.SendAsync("broadcastMessage", "SYSTEM", $"{Context.UserIdentifier} joined.");
            await base.OnConnectedAsync();

        }
    }
}
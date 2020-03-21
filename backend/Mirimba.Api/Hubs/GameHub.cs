using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Hubs
{
    public class GameHub: Hub
    {
        public async Task Send(string roomId, string user, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", new { user = user, message = message });

            await Clients.Group(roomId).SendAsync("ReceiveMessage", new { user = user, message = message });
            
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }
    }
}

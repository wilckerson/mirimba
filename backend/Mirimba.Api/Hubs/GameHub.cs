using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Hubs
{
    public class GameHub: Hub
    {
        public async Task Send(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", new { user = user, message = message });
        }
    }
}

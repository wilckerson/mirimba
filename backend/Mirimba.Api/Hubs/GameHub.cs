using Microsoft.AspNetCore.SignalR;
using Mirimba.Api.Games.Uno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Hubs
{
    public class GameHub : Hub
    {
        static Dictionary<string, UnoGame> gameRooms = new Dictionary<string, UnoGame>();
        static GameHub currentInstance;

        public GameHub()
        {
        }

        //public async Task Send(string roomId, string user, string message)
        //{
        //    //await Clients.All.SendAsync("ReceiveMessage", new { user = user, message = message });
        //    await Clients.Group(roomId).SendAsync("ReceiveMessage", new { user = user, message = message });
        //}

        //public async Task ResetRoom(string roomId)
        //{
        //    gameRooms[roomId] = new UnoGame();
        //    await currentInstance.Clients.Group(roomId).SendAsync("Update", new PlayerState());
        //}


        public async Task JoinRoom(string roomId, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            if (!gameRooms.ContainsKey(roomId))
            {
                //Add new
                gameRooms.Add(roomId, new UnoGame());
            }

            var game = gameRooms[roomId];
            game.AddPlayer(userName);

            var playerState = game.GetPlayerState(userName);

            await Clients.Group(roomId).SendAsync("Update", playerState);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}

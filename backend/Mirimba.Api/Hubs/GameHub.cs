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
        static Dictionary<string, UnoGame> gameConnection = new Dictionary<string, UnoGame>();


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

        public async Task ProcessEvent(string roomId, string eventName, object[] eventArgs)
        {
            var game = gameRooms[roomId];

            if(eventName == "startNewGame")
            {
                game.StartNewGame();
            }           

            await BroadcastUpdateToGamePlayers(game);
        }

        private async Task BroadcastUpdateToGamePlayers(UnoGame game)
        {
            foreach (var player in game.Players)
            {
                var playerState = game.GetPlayerState(player);

                await Clients.Client(player.LastConnectionId).SendAsync("Update", playerState);
            }

            //var playerState = game.GetPlayerState();
            //await Clients.Group(roomId).SendAsync("Update", playerState);
        }

        public async Task JoinRoom(string roomId, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            if (!gameRooms.ContainsKey(roomId))
            {
                //Add new
                gameRooms.Add(roomId, new UnoGame(roomId));
            }

            var game = gameRooms[roomId];

            gameConnection[Context.ConnectionId] = game;

            game.AddPlayer(userName, Context.ConnectionId);

            await BroadcastUpdateToGamePlayers(game);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var game = gameConnection[Context.ConnectionId];
            game.SetOfflinePlayer(Context.ConnectionId);

            BroadcastUpdateToGamePlayers(game).Wait();

            gameConnection.Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}

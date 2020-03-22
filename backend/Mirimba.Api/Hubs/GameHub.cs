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

        public async Task ProcessEvent(string roomId, string userName, string eventName, object[] eventArgs)
        {
            var game = gameRooms[roomId];

            bool needUpdate = true;

            if (eventName == "StartNewGame")
            {
                game.StartNewGame();
            }
            else if (eventName == "GetFromDeck")
            {
                needUpdate = game.GetFromDeckToPlayerHandCards(userName);
            }
            else if (eventName == "GetFromBoard")
            {
                needUpdate = game.GetFromBoardToPlayerHandCards(userName);
            }
            else if (eventName == "ClearBoardPastHistory")
            {
                needUpdate = game.ClearBoardPastHistory();
            }
            else if (eventName == "FromHandToBoard")
            {
                var card = eventArgs.FirstOrDefault()?.ToString();
                game.FromHandToBoard(userName, card);
            }


            if (needUpdate)
            {
                await BroadcastUpdateToGamePlayers(game);
            }
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
            if (gameConnection.TryGetValue(Context.ConnectionId, out UnoGame game))
            {
                game.SetOfflinePlayer(Context.ConnectionId);

                BroadcastUpdateToGamePlayers(game).Wait();

                gameConnection.Remove(Context.ConnectionId);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}

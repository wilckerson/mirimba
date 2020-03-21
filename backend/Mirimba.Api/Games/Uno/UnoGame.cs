using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Games.Uno
{
    public class UnoGame
    {
        public string RoomId { get; private set; }

        private Dictionary<string, Player> players;
        //private Stack<Card> boardCards;
        //private Stack<Card> deck;

        public UnoGame(string roomId)
        {
            RoomId = roomId;
            players = new Dictionary<string, Player>();
            //boardCards = new Stack<Card>();
            //deck = new Stack<Card>();
        }

        public void AddPlayer(string userName, string connectionId)
        {
            Player currenPlayer = null;
            if (players.ContainsKey(userName))
            {
                currenPlayer = players[userName];
            }
            else
            {
                currenPlayer = new Player(userName, connectionId);
                players.Add(userName, currenPlayer);
            }

            currenPlayer.SetOnline(connectionId);
        }

        public void SetOfflinePlayer(string connectionId)
        {
            var player = players.Select(s => s.Value).FirstOrDefault(p => p.LastConnectionId == connectionId);
            if(player != null)
            {
                player.SetOffline();
            }
        }

        public PlayerState GetPlayerState()
        {
            var state = new PlayerState();

            //Other players
            //var onlinePlayers = players.Select(s => s.Value).Where(player => player.IsOnline());
            var allPlayers = players.Select(s => s.Value);
            state.PublicPlayersState = allPlayers.Select(player => new PublicPlayerState()
            {
                UserName = player.UserName,
                HandCardsCount = player.GetHandCardsCount(),
                IsOnline = player.IsOnline()
            }).ToList();

            return state;
        }
    }


}

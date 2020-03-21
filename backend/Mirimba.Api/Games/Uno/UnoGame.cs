using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Games.Uno
{
    public class UnoGame
    {
        private Dictionary<string, Player> players;
        //private Stack<Card> boardCards;
        //private Stack<Card> deck;

        public UnoGame()
        {
            players = new Dictionary<string, Player>();
            //boardCards = new Stack<Card>();
            //deck = new Stack<Card>();
        }

        public void AddPlayer(string userName)
        {
            if(!players.ContainsKey(userName))
            {
                var newPlayer = new Player(userName);
                players.Add(userName, newPlayer);
            }
        }

        public PlayerState GetPlayerState(string userName)
        {
            var state = new PlayerState();

            //Other players
            //var otherPlayers = players.Where(player => player.Key != userName);
            state.PublicPlayersState = players.Select(player => new PublicPlayerState()
            {
                UserName = player.Key,
                HandCardsCount = player.Value.GetHandCardsCount()
            }).ToList();

            return state;            
        }
    }

    
}

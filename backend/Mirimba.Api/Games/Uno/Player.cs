using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Games.Uno
{
    public class Player
    {
        private readonly string userName;
        private List<Card> handCards;

        public Player(string userName)
        {
            this.userName = userName;
            handCards = new List<Card>();
        }

        public int GetHandCardsCount()
        {
            return handCards.Count;
        }

    }
}

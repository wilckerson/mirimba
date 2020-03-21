using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Games.Uno
{
    public class Player
    {
        public string UserName { get; private set; }
        public string LastConnectionId { get; private set; }
        private List<Card> handCards;
        private bool onlineState;

        public Player(string userName, string connectionId)
        {
            this.UserName = userName;
            this.LastConnectionId = connectionId;
            handCards = new List<Card>();
        }

        public int GetHandCardsCount()
        {
            return handCards.Count;
        }

        public void SetOnline(string connectionId)
        {
            this.LastConnectionId = connectionId;
            this.onlineState = true;
        }

        public bool IsOnline()
        {
            return this.onlineState == true;
        }

        public void SetOffline()
        {
            this.onlineState = false;
        }
    }
}

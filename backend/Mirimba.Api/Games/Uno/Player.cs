using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Games.Uno
{
    public class Player
    {
        public string UserName { get; private set; }
        public string LastConnectionId { get; private set; }
        private List<Card> handCards;
        public ReadOnlyCollection<Card> HandCards => new ReadOnlyCollection<Card>(handCards);

        private bool onlineState;
        public bool IsOnline => this.onlineState == true;
       
        public Player(string userName, string connectionId)
        {
            this.UserName = userName;
            this.LastConnectionId = connectionId;
            handCards = new List<Card>();
        }

        public void SetOnline(string connectionId)
        {
            this.LastConnectionId = connectionId;
            this.onlineState = true;
        }

        public void SetOffline()
        {
            this.onlineState = false;
        }

        public void AddToHandCards(Card card)
        {
            handCards.Add(card);
        }

        public Card PopFromHandCards(string cardName)
        {
            var idx = handCards.FindIndex(card => card.Description == cardName);
            if(idx >= 0)
            {
                var card = handCards[idx];
                handCards.RemoveAt(idx);
                return card;
            }

            return null;
        }

        public void ResetHandCards()
        {
            handCards = new List<Card>();
        }
    }
}

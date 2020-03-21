using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Games.Uno
{
    public class PlayerState
    {
        public List<PublicPlayerState> PublicPlayersState { get; set; }
        public List<Card> HandCards { get; set; }
        public int DeckCount { get; set; }
        public List<Card> BoardCards { get; set; }
    }
}

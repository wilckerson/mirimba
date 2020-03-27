using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Games.Uno
{
    public class PlayerState
    {
        public List<PublicPlayerState> PublicPlayersState { get; set; }
        public List<string> HandCards { get; set; }
        public int DeckCount { get; set; }
        public List<string> BoardCards { get; set; }
        public bool IsGameStarted { get; set; }
        public string LastPlayerToPlay { get; set; }
        public int? LastPlayerAction { get; internal set; }
    }
}

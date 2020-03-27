using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Mirimba.Api.Games.Uno
{
    public class UnoGame
    {
        const int NUM_CARDS_PER_PLAYER = 7;

        public string RoomId { get; private set; }
        public bool IsStarted { get; private set; }

        public ReadOnlyCollection<Player> Players => new ReadOnlyCollection<Player>(players.Select(s => s.Value).Where(w => w.IsOnline).ToList());

        public Player LastPlayerToPlay { get; private set; }
        public PlayerActionEnum? LastPlayerAction { get; private set; }

        private Dictionary<string, Player> players;
        private Stack<Card> boardCards;
        private LinkedList<Card> deck;



        public UnoGame(string roomId)
        {
            RoomId = roomId;
            players = new Dictionary<string, Player>();
            boardCards = new Stack<Card>();
            deck = new LinkedList<Card>();
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
            var player = Players.FirstOrDefault(p => p.LastConnectionId == connectionId);
            if (player != null)
            {
                player.SetOffline();
            }
        }



        public PlayerState GetPlayerState(Player currentPlayer)
        {
            var state = new PlayerState();
            state.LastPlayerToPlay = LastPlayerToPlay?.UserName;
            state.LastPlayerAction = LastPlayerAction.HasValue ? (int)LastPlayerAction : new int?();
            state.IsGameStarted = IsStarted;
            state.DeckCount = deck.Count;
            state.BoardCards = boardCards.Select(s => s.Description).ToList();

            //PublicPlayersState
            state.PublicPlayersState = Players.Select(player => new PublicPlayerState()
            {
                UserName = player.UserName,
                HandCardsCount = player.HandCards.Count,
                IsOnline = player.IsOnline,

            }).ToList();

            if (currentPlayer != null)
            {
                state.HandCards = currentPlayer.HandCards.Select(s => s.Description).ToList();
            }

            return state;
        }

        public void FromHandToBoard(string userName, string cardName)
        {
            if (players.TryGetValue(userName, out Player player))
            {
                var card = player.PopFromHandCards(cardName);
                if (card != null)
                {
                    boardCards.Push(card);
                    LastPlayerToPlay = player;
                    LastPlayerAction = PlayerActionEnum.FromHandToBoard;
                }
            }
        }

        public void StartNewGame()
        {
            //Pega o baralho e embaralha
            var cards = GetDeckSet();
            cards = ShuffleCards(cards);
            deck = new LinkedList<Card>(cards);
            boardCards = new Stack<Card>();
            LastPlayerToPlay = null;
            LastPlayerAction = null;

            //Embaralha a ordem dos jogadores
            ShufflePlayers();

            FirstCardOfBoard();
            InitialCardsToPlayers(NUM_CARDS_PER_PLAYER);

            IsStarted = true;
        }

        private void InitialCardsToPlayers(int countPerPlayer)
        {
            foreach (var player in Players)
            {
                player.ResetHandCards();
                for (int i = 0; i < countPerPlayer; i++)
                {
                    var result = GetFromDeckToPlayerHandCards(player, false);
                    if (result == false)
                    {
                        break;
                    }
                }
            }
        }

        private void FirstCardOfBoard()
        {
            //Pega a primeira carta do baralho e coloca na mesa
            //Não pode ser uma carta especial
            while (deck.Count > 0)
            {
                var card = deck.First.Value;
                deck.RemoveFirst();

                boardCards.Push(card);

                if (card.IsSpecial == false)
                {
                    break;
                }
            }
        }

        public bool GetFromDeckToPlayerHandCards(string userName, bool registerLastPlay)
        {
            if (players.TryGetValue(userName, out Player player))
            {
                return GetFromDeckToPlayerHandCards(player, registerLastPlay);
            }
            return false;
        }

        public bool GetFromDeckToPlayerHandCards(Player player, bool registerLastPlay)
        {
            if (deck.Count == 0) { return false; }

            var card = deck.First.Value;
            deck.RemoveFirst();
            player.AddToHandCards(card);

            if (registerLastPlay)
            {
                LastPlayerToPlay = player;
                LastPlayerAction = PlayerActionEnum.GetFromDeck;
            }

            return true;
        }

        public bool GetFromBoardToPlayerHandCards(string userName)
        {
            if (players.TryGetValue(userName, out Player player))
            {
                return GetFromBoardToPlayerHandCards(player);
            }
            return false;
        }

        public bool GetFromBoardToPlayerHandCards(Player player)
        {
            if (boardCards.Count == 0) { return false; }

            var card = boardCards.Pop();
            player.AddToHandCards(card);

            return true;
        }

        public bool ClearBoardPastHistory()
        {
            //Pega as cartas da mesa (exceto a que está no topo da pilha) e devolve para o Deck de forma embaralhada
            if (boardCards.Count < 2) { return false; }

            var lstBoardCards = boardCards.ToList();
            var topCard = lstBoardCards[0];
            lstBoardCards.RemoveAt(0);
            boardCards.Clear();
            boardCards.Push(topCard);

            lstBoardCards = ShuffleCards(lstBoardCards);

            foreach (var card in lstBoardCards)
            {
                deck.AddLast(card);
            }

            return true;
        }

        private List<Card> ShuffleCards(List<Card> cards)
        {
            if (cards.Count < 2)
            {
                return cards;
            }

            var shuffledCards = cards
                .OrderBy(a => Guid.NewGuid())
                .ToList();

            return shuffledCards;
        }

        private void ShufflePlayers()
        {
            var shuffledPlayers = Players
                .OrderBy(a => Guid.NewGuid())
                .ToList();

            this.players.Clear();
            foreach (var player in shuffledPlayers)
            {
                this.players.Add(player.UserName, player);
            }
        }

        private List<Card> GetDeckSet()
        {
            //http://copag.com.br/wp-content/uploads/2016/03/UNO.pdf

            return new List<Card>()
            {
                new Card("red0"),
                new Card("red1"),
                new Card("red2"),
                new Card("red3"),
                new Card("red4"),
                new Card("red5"),
                new Card("red6"),
                new Card("red7"),
                new Card("red8"),
                new Card("red9"),
                new Card("red1"),
                new Card("red2"),
                new Card("red3"),
                new Card("red4"),
                new Card("red5"),
                new Card("red6"),
                new Card("red7"),
                new Card("red8"),
                new Card("red9"),
                new Card("red-block"),
                new Card("red-block"),
                new Card("red-plus2"),
                new Card("red-plus2"),
                new Card("red-reverse"),
                new Card("red-reverse"),

                new Card("yellow0"),
                new Card("yellow1"),
                new Card("yellow2"),
                new Card("yellow3"),
                new Card("yellow4"),
                new Card("yellow5"),
                new Card("yellow6"),
                new Card("yellow7"),
                new Card("yellow8"),
                new Card("yellow9"),
                 new Card("yellow1"),
                new Card("yellow2"),
                new Card("yellow3"),
                new Card("yellow4"),
                new Card("yellow5"),
                new Card("yellow6"),
                new Card("yellow7"),
                new Card("yellow8"),
                new Card("yellow9"),
                new Card("yellow-block"),
                new Card("yellow-block"),
                new Card("yellow-plus2"),
                new Card("yellow-plus2"),
                new Card("yellow-reverse"),
                new Card("yellow-reverse"),

                new Card("green0"),
                new Card("green1"),
                new Card("green2"),
                new Card("green3"),
                new Card("green4"),
                new Card("green5"),
                new Card("green6"),
                new Card("green7"),
                new Card("green8"),
                new Card("green9"),
                 new Card("green1"),
                new Card("green2"),
                new Card("green3"),
                new Card("green4"),
                new Card("green5"),
                new Card("green6"),
                new Card("green7"),
                new Card("green8"),
                new Card("green9"),
                new Card("green-block"),
                new Card("green-block"),
                new Card("green-plus2"),
                new Card("green-plus2"),
                new Card("green-reverse"),
                new Card("green-reverse"),

                new Card("blue0"),
                new Card("blue1"),
                new Card("blue2"),
                new Card("blue3"),
                new Card("blue4"),
                new Card("blue5"),
                new Card("blue6"),
                new Card("blue7"),
                new Card("blue8"),
                new Card("blue9"),
                 new Card("blue1"),
                new Card("blue2"),
                new Card("blue3"),
                new Card("blue4"),
                new Card("blue5"),
                new Card("blue6"),
                new Card("blue7"),
                new Card("blue8"),
                new Card("blue9"),
                new Card("blue-block"),
                new Card("blue-block"),
                new Card("blue-plus2"),
                new Card("blue-plus2"),
                new Card("blue-reverse"),
                new Card("blue-reverse"),

                new Card("change-color"),
                new Card("change-color"),
                new Card("change-color"),
                new Card("change-color"),
                new Card("plus4"),
                new Card("plus4"),
                new Card("plus4"),
                new Card("plus4"),
            };
        }
    }


}

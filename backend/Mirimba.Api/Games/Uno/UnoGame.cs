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

        public ReadOnlyCollection<Player> Players => new ReadOnlyCollection<Player>(players.Select(s => s.Value).ToList());

        private Dictionary<string, Player> players;
        private Stack<Card> boardCards;
        private Stack<Card> deck;



        public UnoGame(string roomId)
        {
            RoomId = roomId;
            players = new Dictionary<string, Player>();
            boardCards = new Stack<Card>();
            deck = new Stack<Card>();
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
            if(player != null)
            {
                player.SetOffline();
            }
        }

        public PlayerState GetPlayerState(Player currentPlayer)
        {
            var state = new PlayerState();
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

            if(currentPlayer != null)
            {
                state.HandCards = currentPlayer.HandCards.Select(s => s.Description).ToList();
            }

            return state;
        }

        public void StartNewGame()
        {
            //Pega o baralho e embaralha
            var cards = GetDeckSet();
            cards = SuffleCards(cards);
            deck = new Stack<Card>(cards);
            boardCards = new Stack<Card>();

            FirstCardOfBoard();
            InitialCardsToPlayers(NUM_CARDS_PER_PLAYER);

            IsStarted = true;
        }

        private void InitialCardsToPlayers(int countPerPlayer)
        {
            foreach (var player in Players)
            {
                for (int i = 0; i < countPerPlayer; i++)
                {
                    var result = GetFromDeckToPlayerHandCards(player);
                    if(result == false)
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
            while(deck.Count > 0)
            {
                var card = deck.Pop();
                boardCards.Push(card);

                if (card.IsSpecial == false)
                {
                    break;
                }
            }
        }

        public bool GetFromDeckToPlayerHandCards(string userName)
        {
            if(players.TryGetValue(userName, out Player player))
            {
                return GetFromDeckToPlayerHandCards(player);
            }
            return false;
        }

        public bool GetFromDeckToPlayerHandCards(Player player)
        {
            if (deck.Count == 0) { return false; }

            var card = deck.Pop();
            player.AddToHandCards(card);

            return true;
        }

        private List<Card> SuffleCards(List<Card> cards)
        {
            var shuffledCards = cards
                .OrderBy(a => Guid.NewGuid())
                .ToList();

            return shuffledCards;
        }

        private List<Card> GetDeckSet()
        {
            return new List<Card>()
            {
                new Card("0Azul"),
                new Card("1Azul"),
                new Card("2Azul"),
                new Card("3Azul"),
                new Card("4Azul"),
                new Card("5Azul"),
                new Card("6Azul"),
                new Card("7Azul"),
                new Card("8Azul"),
                new Card("9Azul"),
                new Card("+2Azul"),
                new Card("PularAzul"),
                new Card("InverterAzul"),
                new Card("0Vermelho"),
                new Card("1Vermelho"),
                new Card("2Vermelho"),
                new Card("3Vermelho"),
                new Card("4Vermelho"),
                new Card("5Vermelho"),
                new Card("6Vermelho"),
                new Card("7Vermelho"),
                new Card("8Vermelho"),
                new Card("9Vermelho"),
                new Card("+2Vermelho"),
                new Card("PularVermelho"),
                new Card("InverterVermelho"),
                new Card("+4"),
                new Card("MudarCor")
            };
        }
    }


}

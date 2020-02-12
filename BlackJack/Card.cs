using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack
{
    public enum Colour
    {
        Red,
        Black
    }

    public enum Symbol
    {
        Diamond,
        Heart,
        Club,
        Spade
    }

    public class Suite
    {
        private readonly Colour Colour;
        public Symbol Symbol { get; private set; }

        public Suite(Symbol symbol)
        {
            Symbol = symbol;

            if (Symbol == Symbol.Diamond || Symbol == Symbol.Heart)
            {
                Colour = Colour.Red;
            }
            else
            {
                Colour = Colour.Black;
            }
        }
    }

    public class Card
    {
        private Suite Suite;
        public string Rank { get; private set; }

        public override string ToString() => Rank + " of " + Suite.Symbol + "s";

        public Card(Suite suite, int rank)
        {
            Suite = suite;

            switch(rank)
            {
                case 1:
                    Rank = "A";
                    break;
                case 10:
                    Rank = "T";
                    break;
                case 11:
                    Rank = "J";
                    break;
                case 12:
                    Rank = "Q";
                    break;
                case 13:
                    Rank = "K";
                    break;
                default:
                    Rank = rank.ToString();
                    break;
            }
        }

        public static List<Card> GenerateDeck()
        {
            List<Card> deck = new List<Card>();

            foreach (Symbol symbol in Enum.GetValues(typeof(Symbol)))
            {
                for (var j = 1; j <= 13; j++)
                {               
                    deck.Add(new Card(new Suite(symbol), j));
                }
            }

            return deck;
        }
    }

    public class CardShuffler
    {     
        private List<Card> Pool { get; set; }
        private int NumDecks { get; set; }
        private List<Card> Stock { get; set; }
        private List<Card> DiscardPile { get; set; }

        private List<Card> PoolCards()
        {
            List<Card> pool = new List<Card>();

            for (var i = 0; i < NumDecks; i++)
            {
                pool.AddRange(Card.GenerateDeck());
            }

            return pool;
        }

        private void Shuffle()
        {                   
            Random random = new Random();
            int n = Stock.Count;

            for (int i = Stock.Count - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                Card value = Stock[rnd];
                Stock[rnd] = Stock[i];
                Stock[i] = value;
            }          
        }

        public CardShuffler(int numDecks = 5)
        {
            NumDecks = numDecks;
            Pool = PoolCards();
            Stock = new List<Card>(Pool);
            Shuffle();
            DiscardPile = null;
        }

        public void ShowPool()
        {
            foreach (Card card in Pool)
            {
                Console.WriteLine(card.ToString());
            }
        }

        public Card Deal()
        {
            Card topCard = null;

            if (Stock.Any())
            {
                topCard = Stock.Last();

                Stock.RemoveAt(Stock.Count - 1);               
            }

            return topCard;
        }

        public void RetrieveCards(Player player, Player dealer)
        {
            if (DiscardPile == null)
            {
                DiscardPile = new List<Card>();
            }        

            DiscardPile.AddRange(player.Hand);
            player.RetrieveHand();
            DiscardPile.AddRange(dealer.Hand);
            dealer.RetrieveHand();
        }

        public bool RestockRequired()
        {
            return Stock.Count < 8 * NumDecks + 55;
        }

        public void Restock()
        {
            Console.WriteLine("\nRefilling shuffler...\n");
            Stock.AddRange(DiscardPile);
            DiscardPile.Clear();
            Shuffle();
        }
    }
}

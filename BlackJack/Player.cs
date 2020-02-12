using System;
using System.Collections.Generic;

namespace BlackJack
{
    public class Player
    {
        private int wins = 0;
        private int draws = 0;
        private int busts = 0;
        private int losses = 0;

        private bool softAce = false;

        public List<Card> Hand { get; private set; } = new List<Card>();
        public int HandValue { get; private set; } = 0;
        public bool Blackjack { get; protected set; } = false;

        public static void DetermineResult(Player player, Dealer dealer)
        {
            if (player.HandValue <= 21)
            {
                if (player.HandValue > dealer.HandValue || dealer.HandValue > 21)
                {
                    Console.WriteLine("Player WINS!\n");
                    player.wins++;
                    dealer.losses++;

                    if (dealer.HandValue > 21)
                    {
                        dealer.busts++;
                    }
                }
                else if (player.HandValue < dealer.HandValue && dealer.HandValue <= 21)
                {
                    Console.WriteLine("Player LOSES!\n");
                    player.losses++;
                    dealer.wins++;
                }
                else
                {
                    Console.WriteLine("TIE!\n");
                    player.draws++;
                    dealer.draws++;
                }
            }
            else
            {
                player.busts++;
                player.losses++;
                dealer.wins++;
            }
        }

        private void AddCardValue(string rank)
        {
            if (int.TryParse(rank, out int value))
            {
                HandValue += value;
            }
            else if (rank == "K" || rank == "Q" || rank == "J" || rank == "T")
            {
                HandValue += 10;
            }
            else
            {
                if (HandValue < 11 && !softAce)
                {
                    HandValue += 11;
                    softAce = true;
                }
                else
                {
                    HandValue++;
                }
            }

            // Change ace in hand (if one) with value of 11 to 1 if hand would bust
            if (HandValue > 21 && softAce)
            {
                HandValue -= 10;
                softAce = false;
            }
        }

        public void AddToHand(Card card, bool faceUp = true)
        {
            if (card != null)
            {
                Hand.Add(card);

                Console.Write("Dealt " + GetType().Name + " card");

                if (faceUp)
                {
                    Console.WriteLine(": " + card.ToString());
                }

                AddCardValue(card.Rank);               
            }
        }

        public void ShowHand()
        {
            Console.WriteLine(GetType().Name + "'s hand is: ");

            foreach (Card card in Hand)
            {
                Console.WriteLine(card.ToString());
            }

            Console.WriteLine("(" + HandValue + ")\n");
        }

        public bool CanHit(bool hitAvailable)
        {
            if (HandValue > 21)
            {
                Console.WriteLine();
                ShowHand();
                Console.WriteLine("BUST");
                Console.WriteLine("Player loses.");
                hitAvailable = false;             
            }

            return hitAvailable;
        }

        protected void ProposeHit(CardShuffler cs)
        {
            Console.WriteLine("Would you like another card?");
            Console.WriteLine("Press 'H' to hit");
            Console.WriteLine("Press 'S' to stand");
            Console.WriteLine("Press 'V' to view your hand\n");

            ConsoleKeyInfo keyInfo;
            bool hitAvailable = true;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.H)
                {
                    AddToHand(cs.Deal());

                    hitAvailable = CanHit(hitAvailable);

                    if (hitAvailable)
                    {
                        Console.WriteLine("\nWould you like another card?");
                        Console.WriteLine("Press 'H' to hit");
                        Console.WriteLine("Press 'S' to stand");
                        Console.WriteLine("Press 'V' to view your hand\n");
                    }
                }

                if (keyInfo.Key == ConsoleKey.V)
                {
                    ShowHand();
                }

                if (keyInfo.Key == ConsoleKey.S)
                {
                    Console.WriteLine("Player stands\n");
                }
            }
            while (keyInfo.Key != ConsoleKey.S && hitAvailable);
        }

        public void CheckBlackjack()
        {
            if (HandValue == 21)
            {
                Console.WriteLine("BLACKJACK!\n");
                Blackjack = true;

                Console.WriteLine("Press 'Enter' to continue");

                ConsoleKeyInfo keyInfo;

                do
                {
                    keyInfo = Console.ReadKey(true);
                }
                while (keyInfo.Key != ConsoleKey.Enter);
            }
        }

        public void Turn(CardShuffler cs)
        {
            if (HandValue < 21)
            {
                ProposeHit(cs);
            }
            else
            {
                CheckBlackjack();
            }
        }

        public void RetrieveHand()
        {
            Hand.Clear();
            HandValue = 0;
            Blackjack = false;
        }

        public void ShowRecord()
        {
            Console.WriteLine(GetType().Name + " Record:");
            Console.WriteLine("W: " + wins);
            Console.WriteLine("D: " + draws);
            Console.WriteLine("B: " + busts);
            Console.WriteLine("L: " + losses + "\n");
        }
    }

    public class Dealer : Player
    {
        public new void Turn(CardShuffler cs)
        {       
            ConsoleKeyInfo keyInfo;         
        
            if (HandValue < 17)
            {
                do
                {
                    Console.WriteLine("Dealer hits");
                    AddToHand(cs.Deal());
                    Console.WriteLine();
                    ShowHand();

                    if (HandValue < 17)
                    {
                        Console.WriteLine("Press 'Enter' for dealer's next card\n");
                    }
                    else if (HandValue > 21)
                    {
                        Console.WriteLine("BUST\n");
                        Console.WriteLine("Press 'Enter' to continue\n");
                    }
                    else
                    {
                        Console.WriteLine("Dealer stands.\n");
                        Console.WriteLine("Press 'Enter' to continue\n");
                    }

                    do
                    {
                        keyInfo = Console.ReadKey(true);
                    }
                    while (keyInfo.Key != ConsoleKey.Enter);
                }
                while (HandValue < 17);
            }
            else
            {
                Console.WriteLine("Dealer stands.\n");
                Console.WriteLine("Press 'Enter' to continue\n");

                do
                {
                    keyInfo = Console.ReadKey(true);
                }
                while (keyInfo.Key != ConsoleKey.Enter);
            }
        }
    }
}

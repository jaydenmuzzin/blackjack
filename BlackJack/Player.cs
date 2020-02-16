using System;
using System.Collections.Generic;
using System.Threading;

namespace BlackJack
{
    public class Player
    {
        private int wins = 0;
        private int draws = 0;
        private int busts = 0;
        private int losses = 0;

        private bool softAce = false;

        protected int WaitTime { get; set; }
        public List<Card> Hand { get; private set; } = new List<Card>();
        public int HandValue { get; private set; } = 0;
        public bool Blackjack { get; protected set; } = false;

        public Player(int _waitTime = 1000)
        {
            WaitTime = _waitTime;
        }

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
                else
                {
                    Console.WriteLine();
                }
               
                Thread.Sleep(WaitTime);

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
                Console.WriteLine("BUST");
                Console.WriteLine("Player loses.\n");
                Console.WriteLine("Press 'Enter' to continue\n");
                ConsoleKeyInfo keyInfo;

                do
                {
                    keyInfo = Console.ReadKey(true);
                }
                while (keyInfo.Key != ConsoleKey.Enter);

                hitAvailable = false;             
            }
            else if (HandValue == 21)
            {
                hitAvailable = false;
            }

            return hitAvailable;
        }

        public virtual void Turn(CardShuffler cs)
        {
            Console.WriteLine("Would you like another card?");
            Console.WriteLine("Press 'H' to hit");
            Console.WriteLine("Press 'S' to stand\n");           

            ConsoleKeyInfo keyInfo;
            bool hitAvailable = true;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.H)
                {
                    AddToHand(cs.Deal());
                    Console.WriteLine();
                    ShowHand();

                    Thread.Sleep(WaitTime);

                    hitAvailable = CanHit(hitAvailable);

                    if (hitAvailable)
                    {
                        Console.WriteLine("Would you like another card?");
                        Console.WriteLine("Press 'H' to hit");
                        Console.WriteLine("Press 'S' to stand\n");
                    }
                }               

                if (keyInfo.Key == ConsoleKey.S)
                {
                    Console.WriteLine("Player stands.\n");
                }
            }
            while (keyInfo.Key != ConsoleKey.S && hitAvailable);
        }

        public void CheckBlackjack()
        {         
            if (HandValue == 21)
            {
                ShowHand();
                Console.WriteLine("BLACKJACK!\n");
                Blackjack = true;
                Console.WriteLine("Press 'Enter' to continue\n");

                ConsoleKeyInfo keyInfo;

                do
                {
                    keyInfo = Console.ReadKey(true);
                }
                while (keyInfo.Key != ConsoleKey.Enter);
            }
        }

        public void RetrieveHand()
        {
            Hand.Clear();
            HandValue = 0;
            Blackjack = false;
            softAce = false;
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
        public Dealer(int _waitTime = 1000)
        {
            WaitTime = _waitTime;
        }

        public override void Turn(CardShuffler cs)
        {
            bool showHitText = true;
            ConsoleKeyInfo keyInfo;         
        
            if (HandValue < 17)
            {               
                do
                {
                    if (showHitText)
                    {
                        Console.WriteLine("Dealer hits.\n");
                        Thread.Sleep(WaitTime);
                    }                                     

                    AddToHand(cs.Deal());
                    Console.WriteLine();
                    ShowHand();

                    Thread.Sleep(WaitTime);

                    if (HandValue < 17)
                    {
                        Console.WriteLine("Press 'Enter' for dealer's next card\n");
                        showHitText = false;
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

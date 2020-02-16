using System;
using System.Threading;

namespace BlackJack
{
    class Program
    {
        private static bool playing = false;
        private static bool settings = false;

        static void NewRound(CardShuffler cs, Player player, Dealer dealer, int numRound)
        {                 
            Console.WriteLine("Round " + numRound + "\n");

            if (cs.RestockRequired())
            {               
                cs.Restock();
            }       

            player.AddToHand(cs.Deal());
            dealer.AddToHand(cs.Deal());
            player.AddToHand(cs.Deal());
            dealer.AddToHand(cs.Deal(), false);

            Console.WriteLine();
            dealer.CheckBlackjack();
            player.CheckBlackjack();

            ConsoleKeyInfo keyInfo;

            if (!dealer.Blackjack)
            {
                if (player.Blackjack)
                {
                    dealer.ShowHand();

                    Console.WriteLine("Press 'Enter' to continue\n");

                    do
                    {
                        keyInfo = Console.ReadKey(true);
                    }
                    while (keyInfo.Key != ConsoleKey.Enter);
                }
                else
                {
                    player.ShowHand();
                    player.Turn(cs);
                }

                if (player.HandValue <= 21 && !player.Blackjack)
                {
                    Thread.Sleep(Settings.WaitTime);

                    Console.WriteLine("Player has been dealt.\n");

                    Thread.Sleep(Settings.WaitTime);

                    Console.WriteLine("Dealer's turn");
                    Console.WriteLine("Press 'Enter' to continue\n");

                    do
                    {
                        keyInfo = Console.ReadKey(true);
                    }
                    while (keyInfo.Key != ConsoleKey.Enter);

                    dealer.ShowHand();

                    Thread.Sleep(Settings.WaitTime);

                    dealer.Turn(cs);

                    if (!dealer.Blackjack && dealer.HandValue <= 21)
                    {
                        Console.WriteLine("Player: " + player.HandValue);
                        Console.WriteLine("Dealer: " + dealer.HandValue);
                        Console.WriteLine();
                    }
                }            
            }
            else
            {             
                if (!player.Blackjack)
                {
                    player.ShowHand();
                    Console.WriteLine("Press 'Enter' to continue\n");

                    do
                    {
                        keyInfo = Console.ReadKey(true);                      
                    }
                    while (keyInfo.Key != ConsoleKey.Enter);
                }
            }

            Player.DetermineResult(player, dealer);

            Thread.Sleep(Settings.WaitTime);

            player.ShowRecord();

            if (Settings.ShowDealerRecord)
            {
                dealer.ShowRecord();
            }
        }

        static void StartGame()
        {
            playing = true;
            int numRound = 1;         

            Player player = new Player(Settings.WaitTime);
            Dealer dealer = new Dealer(Settings.WaitTime);
            CardShuffler cs = new CardShuffler(Settings.NumDecks);

            Console.Clear();

            do
            {
                if (numRound > 1)
                {                  
                    cs.RetrieveCards(player, dealer);
                }             

                NewRound(cs, player, dealer, numRound);            

                Console.WriteLine("Would you like to play another round?");
                Console.WriteLine("Press 'Y' to start a new round");
                Console.WriteLine("Press 'Esc' to exit the game\n");

                ConsoleKeyInfo keyInfo;

                do
                {
                    keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.Y)
                    {
                        numRound++;

                        if (Settings.KeepRoundHistory)
                        {                                                                               
                            Console.WriteLine("========================================");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.Clear();
                        }
                    }

                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        playing = false;
                    }                
                }
                while (keyInfo.Key != ConsoleKey.Y && keyInfo.Key != ConsoleKey.Escape);

            }
            while (playing == true);

            Console.Clear();
            Console.WriteLine("Game ended\n");
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("Welcome to BlackJack!");
            Console.WriteLine("Press 'P' to play");
            Console.WriteLine("Press 'S' to settings");
            Console.WriteLine("Press 'Esc' to exit\n");

            ConsoleKeyInfo keyInfo;

            do
            {            
                keyInfo = Console.ReadKey(true);               

                if (keyInfo.Key == ConsoleKey.P)
                {                
                    playing = true;
                }

                if (keyInfo.Key == ConsoleKey.S)
                {
                    settings = true;
                }
            }
            while (keyInfo.Key != ConsoleKey.P && keyInfo.Key != ConsoleKey.S && keyInfo.Key != ConsoleKey.Escape);

            Console.Clear();
        }

        static void ShowSettingsMenu()
        {
            bool showSettingsInfo = true;
            ConsoleKeyInfo keyInfo;

            Console.WriteLine("Settings\n");

            do
            {
                if (showSettingsInfo)
                {                  
                    Settings.Show();

                    Console.WriteLine("Press 'T' to change the game theme");
                    Console.WriteLine("Press 'N' to change the number of decks used");
                    Console.WriteLine("Press 'W' to change the wait time between events");
                    Console.WriteLine("Press 'H' to toggle the round history");
                    Console.WriteLine("Press 'R' to toggle the dealer record");
                    Console.WriteLine("Press 'D' to reset defaults");
                    Console.WriteLine("Press 'Esc' to return to the main menu\n");
                    showSettingsInfo = false;
                }

                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.T)
                {
                    Settings.ThemeSetting();
                    showSettingsInfo = true;
                }

                if (keyInfo.Key == ConsoleKey.N)
                {
                    Settings.NumDecksSetting();
                    showSettingsInfo = true;
                }

                if (keyInfo.Key == ConsoleKey.W)
                {
                    Settings.WaitTimeSetting();
                    showSettingsInfo = true;
                }

                if (keyInfo.Key == ConsoleKey.H)
                {
                    Settings.RoundHistorySetting();
                    showSettingsInfo = true;
                }

                if (keyInfo.Key == ConsoleKey.R)
                {
                    Settings.DealerRecordSetting();
                    showSettingsInfo = true;
                }

                if (keyInfo.Key == ConsoleKey.D)
                {
                    Settings.ResetDefaults();
                    showSettingsInfo = true;
                }

                Console.Clear();
            }
            while (keyInfo.Key != ConsoleKey.Escape);

            settings = false;
        }

        static bool ShowExitMenu()
        {
            Console.WriteLine("Are you sure you want to quit?");
            Console.WriteLine("Press 'Y' to quit");
            Console.WriteLine("Press 'N' to resume\n");

            ConsoleKeyInfo keyInfo;
            bool exitChoice = false;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Y)
                {
                    exitChoice = true;
                }
            }
            while (keyInfo.Key != ConsoleKey.Y && keyInfo.Key != ConsoleKey.N);

            Console.Clear();

            return exitChoice;
        }

        static void Main(string[] args)
        {
            bool run = true;

            while (run)
            {
                ShowMainMenu();

                if (playing)
                {
                    StartGame();
                }
                else if (settings)
                {
                    ShowSettingsMenu();
                }
                else
                {
                    bool quit = false;
                    quit = ShowExitMenu();

                    if (quit)
                    {
                        run = false;
                    }
                }         
            }

            return;
        }
    }
}

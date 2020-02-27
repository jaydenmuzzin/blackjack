using System;
using System.Collections.Generic;
using System.Threading;

namespace BlackJack
{
    class Program
    {
        private static bool playing = false;
        private static bool settings = false;

        static void NewRound(CardShuffler cs, List<Player> players, Dealer dealer, int numRound)
        {                 
            Console.WriteLine("Round " + numRound + "\n");

            if (cs.RestockRequired())
            {               
                cs.Restock();
            }

            // First round of dealing
            foreach (Player player in players)
            {
                player.AddToHand(cs.Deal());
            }

            dealer.AddToHand(cs.Deal());

            // Second round of dealing
            foreach (Player player in players)
            {
                player.AddToHand(cs.Deal());
            }

            dealer.AddToHand(cs.Deal(), false);

            Console.WriteLine();

            // Check if anyone has blackjack
            dealer.CheckBlackjack();

            foreach (Player player in players)
            {
                player.CheckBlackjack();
            }

            ConsoleKeyInfo keyInfo;          
            int bustCount = 0;

            if (!dealer.Blackjack)
            {
                foreach (Player player in players)
                {
                    if (!player.Blackjack)
                    {
                        Console.WriteLine(player.Name + "'s turn.\n");
                        Thread.Sleep(Settings.WaitTime);
                        player.ShowHand();
                        player.Turn(cs);
                        
                        if (player.HandValue > 21)
                        {
                            bustCount++;
                        }

                        Thread.Sleep(Settings.WaitTime);
                        Console.WriteLine(player.Name + " has been dealt.\n");
                        Thread.Sleep(Settings.WaitTime);                     
                    }                        
                }

                if (bustCount != players.Count)
                {
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
                }
                else
                {
                    Console.WriteLine("All players BUST!\n");
                    Console.WriteLine("Press 'Enter' to continue\n");
                }
                
            }
            else
            {
                foreach (Player player in players)
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
            }

            if (!dealer.Blackjack && dealer.HandValue <= 21)
            {
                foreach (Player player in players)
                {
                    Console.WriteLine(player.Name + ": " + player.HandValue);
                }

                Console.WriteLine();

                if (bustCount != players.Count)
                {
                    Console.WriteLine("Dealer: " + dealer.HandValue);
                    Console.WriteLine();
                }
            }

            foreach (Player player in players)
            {
                Player.DetermineResult(player, dealer);
                Thread.Sleep(Settings.WaitTime);
                player.ShowRecord();
                Thread.Sleep(Settings.WaitTime);
            }          

            if (Settings.ShowDealerRecord)
            {
                dealer.ShowRecord();
            }

            cs.RetrieveCards(new List<Player>(players) { dealer });
        }

        static void StartGame()
        {
            playing = true;
            int numRound = 1;

            List<Player> players = new List<Player>();
            int numPlayers = 1;

            ConsoleKeyInfo keyInfo;

            Console.WriteLine("Enter the number of players, 1 to 9, then press 'Enter' to start");

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key != ConsoleKey.Enter)
                {
                    numPlayers = Settings.NumPicker(keyInfo.KeyChar, numPlayers);
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine("\nNumber of players in game: " + numPlayers);

            for (int i = 1; i <= numPlayers; i++)
            {
                string playerName = "Player " + i;        

                if (numPlayers > 1)
                {
                    Console.WriteLine("\nWould Player " + i + " like a name? Press 'Y' for yes or 'N' for no");
                }
                else
                {
                    Console.WriteLine("\nWould you like a name? Press 'Y' for yes or 'N' for no");
                }

                do
                {               
                    keyInfo = Console.ReadKey(true);   
                    
                    if (keyInfo.Key != ConsoleKey.Y && keyInfo.Key != ConsoleKey.N)
                    {
                        Console.WriteLine("\nPlease enter 'Y' to name player or 'N' to continue without naming player");
                    }
                }
                while (keyInfo.Key != ConsoleKey.Y && keyInfo.Key != ConsoleKey.N);

                if (keyInfo.Key == ConsoleKey.Y)
                {
                    if (numPlayers > 1)
                    {
                        Console.WriteLine("\nType Player " + i + "'s name, then press 'Enter' to continue");
                    }
                    else
                    {
                        Console.WriteLine("\nType your name, then press 'Enter' to continue");
                    }

                    playerName = Console.ReadLine();              
                }

                players.Add(new Player(playerName, Settings.WaitTime));
            }          

            Dealer dealer = new Dealer(Settings.WaitTime);            
            CardShuffler cs = new CardShuffler(Settings.NumDecks);

            Console.Clear();

            Player.ShowPlayers(players);
            Thread.Sleep(Settings.WaitTime);

            do
            {
                NewRound(cs, players, dealer, numRound);            

                Console.WriteLine("Would you like to play another round?");
                Console.WriteLine("Press 'Y' to start a new round");
                Console.WriteLine("Press 'Esc' to exit the game\n");

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

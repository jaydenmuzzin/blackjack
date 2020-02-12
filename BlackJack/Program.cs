using System;

namespace BlackJack
{
    class Program
    {
        private static bool playing = false;
        private static bool settings = false;

        static void NewRound(CardShuffler cs, Player player, Dealer dealer)
        {
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

            if (!dealer.Blackjack)
            {
                Console.WriteLine();

                player.ShowHand();

                player.Turn(cs);

                ConsoleKeyInfo keyInfo;

                if (player.HandValue <= 21 && !player.Blackjack)
                {
                    player.ShowHand();
                    Console.WriteLine("Player has been dealt\n");
                    Console.WriteLine("Dealer's turn");
                    Console.WriteLine("Press 'Enter' to continue\n");

                    do
                    {
                        keyInfo = Console.ReadKey(true);
                    }
                    while (keyInfo.Key != ConsoleKey.Enter);

                    dealer.ShowHand();

                    dealer.Turn(cs);

                    if (!dealer.Blackjack)
                    {
                        Console.WriteLine("Player: " + player.HandValue);
                        Console.WriteLine("Dealer: " + dealer.HandValue);
                    }
                }

                Console.WriteLine();
            }
            else
            {
                player.CheckBlackjack();
            }

            Player.DetermineResult(player, dealer);
            player.ShowRecord();
            dealer.ShowRecord();
        }

        static void StartGame()
        {
            int numRounds = 1;
            playing = true;

            Player player = new Player();
            Dealer dealer = new Dealer();
            CardShuffler cs = new CardShuffler(Settings.NumDecks);

            do
            {
                if (numRounds > 1)
                {                  
                    cs.RetrieveCards(player, dealer);
                }             

                NewRound(cs, player, dealer);            

                Console.WriteLine("Would you like to play another round?");
                Console.WriteLine("Press 'Y' to start a new round");
                Console.WriteLine("Press 'Esc' to exit the game\n");

                ConsoleKeyInfo keyInfo;

                do
                {
                    keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.Y)
                    {
                        numRounds++;
                    }

                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        playing = false;
                    }                
                }
                while (keyInfo.Key != ConsoleKey.Y && keyInfo.Key != ConsoleKey.Escape);

            }
            while (playing == true);

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
        }

        static void ShowSettingsMenu()
        {
            bool showSettingsInfo = true;
            ConsoleKeyInfo keyInfo;

            do
            {
                if (showSettingsInfo)
                {
                    Console.WriteLine("Settings");
                    Console.WriteLine("Press 'N' to change the number of decks used");
                    Console.WriteLine("Press 'T' to change the game theme");
                    Console.WriteLine("Press 'D' to reset defaults");
                    Console.WriteLine("Press 'Esc' to return to the main menu\n");

                    showSettingsInfo = false;
                }

                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.N)
                {
                    Settings.NumDecksSetting();
                    showSettingsInfo = true;
                }

                if (keyInfo.Key == ConsoleKey.T)
                {
                    Settings.ThemeSetting();
                    showSettingsInfo = true;
                }

                if (keyInfo.Key == ConsoleKey.D)
                {
                    Settings.ResetDefaults();
                    showSettingsInfo = true;
                }

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

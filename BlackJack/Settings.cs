using System;

namespace BlackJack
{
    public enum ThemeName
    {
        Default,
        Inverse,
        Casino
    }

    public static class Settings
    {
        private static readonly ThemeName defTheme = ThemeName.Default;
        private static readonly int defNumDecks = 5;
        private static readonly int defWaitTime = 1000;
        private static readonly bool defKeepRoundHistory = true;
        private static readonly bool defShowDealerRecord = true;

        public static ThemeName Theme { get; set; } = defTheme;
        public static int NumDecks { get; set; } = defNumDecks;
        public static int WaitTime { get; set; } = defWaitTime;
        public static bool KeepRoundHistory { get; set; } = defKeepRoundHistory;
        public static bool ShowDealerRecord { get; set; } = defShowDealerRecord;

        public static void Show()
        {
            Console.WriteLine("Game theme is: " + Theme);
            Console.WriteLine("Number of decks used: " + NumDecks);
            Console.WriteLine("Wait time between events: " + WaitTime / 1000 + "s");

            if (KeepRoundHistory)
            {
                Console.WriteLine("Round history on");
            }
            else
            {
                Console.WriteLine("Round history off");
            }

            if (ShowDealerRecord)
            {
                Console.WriteLine("Dealer record on");
            }
            else
            {
                Console.WriteLine("Dealer record off");
            }

            Console.WriteLine();
        }

        public static void ChangeTheme(ThemeName theme)
        {
            switch (theme)
            {
                case ThemeName.Inverse:
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    break;

                case ThemeName.Casino:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;

                case ThemeName.Default:                  
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }

            Console.Clear();
            Console.WriteLine(theme.ToString());
            Console.WriteLine("\nPress 'I' for Inverse");
            Console.WriteLine("Press 'C' for Casino");
            Console.WriteLine("Press 'D' for Default");
            Console.WriteLine("Press 'Enter' to confirm the setting");
        }

        public static void ResetTheme()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Theme = defTheme;
        }

        public static void ThemeSetting()
        {
            Console.WriteLine("Game theme is: " + Theme);
            Console.WriteLine("Press 'I' for Inverse");
            Console.WriteLine("Press 'C' for Casino");
            Console.WriteLine("Press 'D' for Default");
            Console.WriteLine("Press 'Enter' to confirm the setting");

            ConsoleKeyInfo keyInfo;
            ThemeName theme = Theme;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.I)
                {
                    theme = ThemeName.Inverse;
                    ChangeTheme(theme);
                }

                if (keyInfo.Key == ConsoleKey.C)
                {
                    theme = ThemeName.Casino;
                    ChangeTheme(theme);
                }

                if (keyInfo.Key == ConsoleKey.D)
                {
                    theme = ThemeName.Default;
                    ChangeTheme(theme);
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);

            Theme = theme;
        }

        private static int NumDecks_KeyPress(char keyChar, int numDecks)
        {
            if (int.TryParse(keyChar.ToString(), out int i))
            {
                if (i > 0)
                {
                    numDecks = i;
                    Console.WriteLine(numDecks);
                }
            }

            return numDecks;
        }

        public static void NumDecksSetting()
        {
            Console.WriteLine("Number of decks used: " + NumDecks);
            Console.WriteLine("Press 1-9 to change the decks used to that number");
            Console.WriteLine("Press 'Enter' to confirm the setting");

            ConsoleKeyInfo keyInfo;
            int numDecks = NumDecks;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key != ConsoleKey.Enter)
                {
                    numDecks = NumDecks_KeyPress(keyInfo.KeyChar, numDecks);
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);

            NumDecks = numDecks;
        }

        private static int WaitSecs_KeyPress(char keyChar, int waitSecs)
        {
            if (int.TryParse(keyChar.ToString(), out int i))
            {
                if (i >= 0 && i <= 3)
                {
                    waitSecs = i;
                    Console.WriteLine(waitSecs);
                }
            }

            return waitSecs;
        }

        public static void WaitTimeSetting()
        {
            Console.WriteLine("Wait time between events: " + WaitTime / 1000 + "s");
            Console.WriteLine("Press 0-3 to change the wait time to that number of seconds");
            Console.WriteLine("Press 'Enter' to confirm the setting");

            ConsoleKeyInfo keyInfo;
            int waitSecs = WaitTime;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key != ConsoleKey.Enter)
                {                   
                    waitSecs = WaitSecs_KeyPress(keyInfo.KeyChar, waitSecs);
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);

            WaitTime = waitSecs * 1000;
        }

        public static void RoundHistorySetting()
        {
            KeepRoundHistory = !KeepRoundHistory;
            Console.Clear();           
        }

        public static void DealerRecordSetting()
        {
            ShowDealerRecord = !ShowDealerRecord;
            Console.Clear();
        }

        public static void ResetDefaults()
        {
            ResetTheme();
            NumDecks = defNumDecks;
            WaitTime = defWaitTime;
            KeepRoundHistory = defKeepRoundHistory;
            ShowDealerRecord = defShowDealerRecord;       
            Console.WriteLine("Settings reset to default\n");
        }
    }
}

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
        public static int DefNumDecks { get; } = 5;
        public static ThemeName DefTheme { get; } = ThemeName.Default;

        public static int NumDecks { get; set; } = DefNumDecks;
        public static ThemeName Theme { get; set; } = DefTheme;

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
            Theme = DefTheme;
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
            Console.WriteLine("\nNumber of decks used: " + NumDecks + "\n");
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
            Console.WriteLine("\nGame theme is: " + Theme.ToString() + "\n");
        }

        public static void ResetDefaults()
        {
            NumDecks = DefNumDecks;
            ResetTheme();

            Console.WriteLine("Number of decks used: " + NumDecks);
            Console.WriteLine("Game theme is: " + Theme + "\n");
            Console.WriteLine("Settings reset to default\n");
        }
    }
}

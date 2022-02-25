using System;
using System.IO;
using System.Diagnostics;



namespace MemoryGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Users!");
            string answer = "";
            Game game = new Game();

            do
            {
                Console.WriteLine("Choose level of game.");
                Console.WriteLine("1 - Easy");
                Console.WriteLine("2 - Difficalt");

                bool isUserSelectLevel = true;

                while (isUserSelectLevel)
                {

                    string userLevelAnswer = Console.ReadLine();

                    if (userLevelAnswer == "1")
                    {
                        game.setEasyLevel();
                        isUserSelectLevel = false;
                    }
                    else if (userLevelAnswer == "2")
                    {
                        game.setHardLevel();
                        isUserSelectLevel = false;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect number. Please, select correct level of game.");
                    }
                }

                game.setData();
                game.startGame();
                game.showBestResults();

                Console.WriteLine("Do you want to start again? (Yes/No)");
                do
                {
                    answer = Console.ReadLine().ToUpper();
                } while (answer != "YES" && answer != "NO");
            } while (answer == "YES");

            Console.WriteLine("Thanks, bye!");
        }
    }
}
       
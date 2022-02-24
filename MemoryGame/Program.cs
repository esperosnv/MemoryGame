using System;
using System.IO;
using System.Diagnostics;



namespace MemoryGame
{
    class Program
    {

        static int numberOfCards = 8;
        static string[] wordsList = new string[numberOfCards * 2];
        static Card[] cards = new Card[numberOfCards * 2];
        static int padConst = 15;
        static int scores = 0;
        static int maxLevelChances = 0;
        static Stopwatch sw = new Stopwatch();
       




        static void Main(string[] args)
        {

            Console.WriteLine("Hello Users!");
            string answer = "";


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
                        numberOfCards = 4;
                        maxLevelChances = 10;
                        isUserSelectLevel = false;
                    }
                    else if (userLevelAnswer == "2")
                    {
                        numberOfCards = 8;
                        isUserSelectLevel = false;
                        maxLevelChances = 15;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect number. Please, select correct level of game.");
                    }
                }

                scores = 0;
                setDate();
                sw.Restart();
                startGame(maxLevelChances);

                Console.WriteLine("Do you want to start again? (Yes/No)");

                do
                {
                    answer = Console.ReadLine().ToUpper();
                    Console.WriteLine("yes or no");
                } while (answer != "YES" && answer != "NO");
   

            } while (answer == "YES");

        }





        static void ReadFile()
        {
            var words = File.ReadAllLines("/Users/nadzieja/Projects/consoleApp/Words.txt");

            var rand = new Random();
            List<int> listNumbers = new List<int>();


            int number;
            for (int i = 0; i < numberOfCards; i++)
            {
                do
                {
                    number = rand.Next(0, words.Length - 1);
                } while (listNumbers.Contains(number));

                listNumbers.Add(number);
                wordsList[2 * i] = words[number];
                wordsList[2 * i + 1] = words[number];

            }

            wordsList = Shuffle(wordsList);

        }


        static string[] Shuffle(string[] wordArray)
        {
            Random random = new Random();

            for (int i = (2 * numberOfCards - 1); i > 0; i--)
            {
                int swapIndex = random.Next(i + 1);
                string temp = wordArray[i];
                wordArray[i] = wordArray[swapIndex];
                wordArray[swapIndex] = temp;
            }
            return wordArray;
        }


        static void setDate()
        {
            ReadFile();

            for (int i = 0; i < (numberOfCards * 2); i += 1)
            {

                Card card = new Card();
                card.word = wordsList[i];
                card.position = i;
               // Console.WriteLine(card.word + " " + card.position + " " + card.isOpen);
                cards[i] = card;

            }
        }

        static void printCards(Card[] cards, int chancesTaken)
        {

            Console.Clear();
            Console.WriteLine("Your scores = " + scores);
            Console.WriteLine("Your have " + (maxLevelChances - chancesTaken) + " attemps");
            Console.WriteLine("-".PadRight(5) + "1".PadRight(padConst) + "2".PadRight(padConst) + "3".PadRight(padConst) + "4".PadRight(padConst));

            for (int i = 0; i < (numberOfCards * 2); i += 1)
            {
                switch (i)
                {
                    case 0:
                        Console.Write("A".PadRight(5));
                        break;
                    case 4:
                        Console.Write("B".PadRight(5));
                        break;
                    case 8:
                        Console.Write("C".PadRight(5));
                        break;
                    case 12:
                        Console.Write("D".PadRight(5));
                        break;
                    default:
                        break;
                }

                if (cards[i].isOpen)
                {
                    Console.Write(cards[i].word.PadRight(padConst));
                }
                else
                {
                    Console.Write("X".PadRight(padConst));
                }

                if (i % 4 == 3)
                {
                    Console.WriteLine();
                }
            }
        }





        static string checkCoordinates(string coordinate)
        {
            while (coordinate.Length != 2 || !Char.IsLetter(coordinate[0]) || !Char.IsDigit(coordinate[1])
                || Convert.ToInt32(coordinate.Substring(1, 1)) > 4 || Convert.ToInt32(coordinate.Substring(1, 1)) < 1
                || !isCorrectLetter(coordinate))

            {
                Console.WriteLine("Sorry, you type in incorrect card. Please, repeate again. checkCoordinates");
                coordinate = Console.ReadLine();

            }

            return coordinate;

        }



        static void startGame(int attemps)
        {


            for (int chancesTaken = 0; chancesTaken < attemps; chancesTaken++)
            {
           
                printCards(cards, chancesTaken);

                Console.WriteLine("Please select the first card:");
                string firstCoordinate = Console.ReadLine();

              
                firstCoordinate = checkCoordinates(firstCoordinate);
                int firstCardPosition = calculateCardPosition(firstCoordinate);
                Console.Clear();
                printCards(cards, chancesTaken);

                string secondCoordinate;
                do
                {
                    Console.WriteLine("Please select the second card:");
                    secondCoordinate = Console.ReadLine();
                    secondCoordinate = checkCoordinates(secondCoordinate);
                    if (secondCoordinate == firstCoordinate)
                    {
                        Console.WriteLine("You typed in the same position");
                    }
                } while (secondCoordinate == firstCoordinate);


                int secondCardPosition = calculateCardPosition(secondCoordinate);
                Console.Clear();
                printCards(cards, chancesTaken);


                if ((cards[firstCardPosition].word == cards[secondCardPosition].word) && (firstCardPosition != secondCardPosition))
                {

                    Console.WriteLine("Угадали!");
                    cards[firstCardPosition].isGuessRight = true;
                    cards[secondCardPosition].isGuessRight = true;
                    scores++;
                    if (scores == numberOfCards)
                    {
                        Console.WriteLine("Congratulations! You win this game!");

                        sw.Stop();
                        int time = (int)sw.Elapsed.TotalSeconds;
                        int minutes = time / 60;
                        int seconds = time % 60;

                        Console.WriteLine("You solved the memory game after " + ( chancesTaken + 1) + ". It took you " + minutes + " minutes " + seconds + " seconds.");


                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Увы, мимо!");

                    if (cards[firstCardPosition].isGuessRight == false)
                    {
                        cards[firstCardPosition].isOpen = false;
                    }
                    if (cards[secondCardPosition].isGuessRight == false)
                    {
                        cards[secondCardPosition].isOpen = false;
                    }
                }

                Console.WriteLine("To continue press Enter");

                while (Console.ReadLine() != "") { };
            }
            Console.WriteLine("You lost the game.");
        }

        static int calculateCardPosition(String coordinate)
        {
            Console.Clear();
            string letter = coordinate.Substring(0, 1).ToUpper();
            Console.WriteLine(coordinate);
            int number = Convert.ToInt32(coordinate.Substring(1, 1));
            int index = new int();

            switch (letter)
            {
                case "A":
                    index = number - 1;
                    break;
                case "B":
                    index = 3 + number;
                    break;
                case "C":
                    index = 7 + number;
                    break;
                case "D":
                    index = 11 + number;
                    break;
                default:
                    break;
            }
            Console.WriteLine(cards[index].word);


            if (cards[index].isGuessRight == false)
            {
                cards[index].isOpen = !cards[index].isOpen;
            }

           // Console.WriteLine(cards[index].isOpen);
           // Console.WriteLine(cards[index].isGuessRight);

            return index;

        }

        static bool isCorrectLetter(string coordinate)
        {
           bool result = true;

            switch (numberOfCards)
            {
                case 4:

                    
                    if ((coordinate.Substring(0, 1).ToUpper() != "A")
                        && (coordinate.Substring(0, 1).ToUpper() != "B"))
                    {
                        result = false;
                    }
                    break;
                case 8:
                    if ((coordinate.Substring(0, 1).ToUpper() != "A")
                        && (coordinate.Substring(0, 1).ToUpper() != "B")
                        && (coordinate.Substring(0, 1).ToUpper() != "C")
                        && (coordinate.Substring(0, 1).ToUpper() != "D"))
                    {
                        result = false;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

    }
}
       
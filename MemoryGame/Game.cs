using System;
using System.IO;
using System.Diagnostics;


namespace MemoryGame
{
	public class Game
	{

        static int numberOfCards = 8;
        static string[] wordsList = new string[numberOfCards * 2];
        static Card[] cards = new Card[numberOfCards * 2];
        static List<UserResult> highScore = new List<UserResult>();
        static int padConst = 15;
        static int scores = 0;
        static int maxLevelChances = 0;
        static Stopwatch sw = new Stopwatch();


        public void setEasyLevel()
        {
            numberOfCards = 4;
            maxLevelChances = 10;
        }

        public void setHardLevel()
        {
            numberOfCards = 8;
            maxLevelChances = 15;
        }

        public void setData()
        {
            ReadFile();

            for (int i = 0; i < (numberOfCards * 2); i += 1)
            {
                Card card = new Card();
                card.word = wordsList[i];
                card.position = i;
                cards[i] = card;
            }
        }



        public void startGame()
        {
            sw.Restart();
            scores = 0;
            Console.WriteLine("maxLevelChances " + maxLevelChances);
            for (int chancesTaken = 0; chancesTaken < maxLevelChances; chancesTaken++)
            {

                printCards(cards, chancesTaken);
                string firstCoordinate;
                int firstCardPosition;

                do
                {
                    Console.WriteLine("Please select the first card:");
                    firstCoordinate = Console.ReadLine();

                    firstCoordinate = checkCoordinates(firstCoordinate);
                    firstCardPosition = calculateCardPosition(firstCoordinate);
                    if (cards[firstCardPosition].isGuessRight)
                    {
                        Console.WriteLine("This card is already guessed. Try another card.");
                    }
                } while (cards[firstCardPosition].isGuessRight);

                flipCard(firstCardPosition);
                printCards(cards, chancesTaken);

                string secondCoordinate;
                int secondCardPosition;
                do
                {
                    Console.WriteLine("Please select the second card:");
                    secondCoordinate = Console.ReadLine();

                    secondCoordinate = checkCoordinates(secondCoordinate);
                    if (secondCoordinate == firstCoordinate)
                    {
                        Console.WriteLine("You typed in the same position");
                    }

                    secondCardPosition = calculateCardPosition(secondCoordinate);
                    if (cards[secondCardPosition].isGuessRight)
                    {
                        Console.WriteLine("This card is already guessed. Try another card.");
                    }
                } while ((secondCoordinate == firstCoordinate) || cards[secondCardPosition].isGuessRight);

                flipCard(secondCardPosition);
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

                        Console.WriteLine();
                        Console.WriteLine("You solved the memory game after " + (chancesTaken + 1) + ". It took you " + minutes + " minutes " + seconds + " seconds.");
                        Console.WriteLine();
                        Console.WriteLine("To save your result in the win table, please, write your name.");
                        string userName = checkName();

                        string usersResult = userName + "|" + DateTime.Now.Date.ToShortDateString() + "|" + (chancesTaken + 1) + "|" + time + "\n";
                        string usersResultFilePath = "/Users/nadzieja/Projects/consoleApp/UserResults.txt";


                        if (!File.Exists(usersResultFilePath))
                        {
                            File.WriteAllText(usersResultFilePath, String.Empty);
                        }
                        File.AppendAllText(usersResultFilePath, usersResult);


                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Увы, мимо!");

                    flipCard(firstCardPosition);
                    flipCard(secondCardPosition);

                }

                Console.WriteLine("To continue press Enter");
                while (Console.ReadLine() != "") { };
            }
            Console.WriteLine("You lost the game.");
        }



        public void showBestResults()
        {
            string[] userResultsFile = File.ReadAllLines("/Users/nadzieja/Projects/consoleApp/UserResults.txt");

            Console.WriteLine("User's best results:");
            Console.WriteLine();
            Console.WriteLine("  " + "Name".PadRight(10) + "Guesses".PadRight(8) + "Seconds".PadRight(8));

            string[] personalData = new string[4];
            char[] divider = { '|' };
            highScore.Clear();

            for (int i = 0; i < userResultsFile.Length; i++)
            {
                UserResult userResult = new UserResult();
                personalData = userResultsFile[i].Split(divider);
                userResult.userName = personalData[0];
                userResult.day = personalData[1];
                userResult.wastedChances = Convert.ToInt32(personalData[2]);
                userResult.seconds = Convert.ToInt32(personalData[3]);
                highScore.Add(userResult);
            }

            highScore.Sort();

            for (int i = 0; i < 10 && i < highScore.Count(); i++)
            {
                Console.WriteLine(((i + 1) + "." + highScore[i].userName).PadRight(12) + highScore[i].wastedChances + "".PadRight(7) + highScore[i].seconds);

            }
            Console.WriteLine();
        }


        private void ReadFile()
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

        private string[] Shuffle(string[] wordArray)
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

        private void printCards(Card[] cards, int chancesTaken)
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


        private string checkCoordinates(string coordinate)
        {
            while (coordinate.Length != 2 || !Char.IsLetter(coordinate[0]) || !Char.IsDigit(coordinate[1])
                || Convert.ToInt32(coordinate.Substring(1, 1)) > 4 || Convert.ToInt32(coordinate.Substring(1, 1)) < 1
                || !isCorrectLetter(coordinate))

            {
                Console.WriteLine("Sorry, you type in incorrect card. Please, repeate again. checkCoordinates");
                coordinate = Console.ReadLine();

            }

            return coordinate.ToUpper();
        }

        private int calculateCardPosition(String coordinate)
        {
            string letter = coordinate.Substring(0, 1).ToUpper();
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

            return index;
        }


        private bool isCorrectLetter(string coordinate)
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



        private string checkName()
        {
            string userName = Console.ReadLine();

            while (userName.Length < 2)
            {
                Console.WriteLine("Too short name. Please, write your name.");
                userName = Console.ReadLine();
            };
            return userName;

        }

        private void flipCard(int position)
        {

            if (cards[position].isGuessRight == false)
            {
                cards[position].isOpen = !cards[position].isOpen;
            }

        }




    }
}


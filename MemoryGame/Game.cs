using System;
using System.IO;
using System.Diagnostics;


namespace MemoryGame
{
	public class Game
	{
        static int numberOfCards = 8;
        string[] wordsList = new string[numberOfCards * 2];
        Card[] cards = new Card[numberOfCards * 2];
        List<UserResult> userResults = new List<UserResult>();
        int pad = 15;
        int score = 0;
        int maxLevelChances = 0;
        Stopwatch sw = new Stopwatch();
        string scoreBoardFile = "ScoreBoard.txt";
        string level;



        public void setEasyLevel()
        {
            numberOfCards = 4;
            maxLevelChances = 10;
            level = "easy";
        }

        public void setHardLevel()
        {
            numberOfCards = 8;
            maxLevelChances = 15;
            level = "hard";
        }



        public void setData(string wordsFile)
        {
            ReadFile(wordsFile);

            for (int i = 0; i < (numberOfCards * 2); i++)
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
            score = 0;

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

                if (cards[firstCardPosition].word == cards[secondCardPosition].word)
                {

                    Console.WriteLine("You guessed!");
                    cards[firstCardPosition].isGuessRight = true;
                    cards[secondCardPosition].isGuessRight = true;
                    score++;
                    if (score == numberOfCards)
                    {
                        Console.WriteLine("Congratulations! You won this game!");

                        sw.Stop();
                        int time = (int)sw.Elapsed.TotalSeconds;
                        int minutes = time / 60;
                        int seconds = time % 60;

                        Console.WriteLine();
                        Console.WriteLine("You solved the memory game after " + (chancesTaken + 1) + " attemps. It took you " + minutes + " minutes " + seconds + " seconds.");
                        Console.WriteLine();
                        Console.WriteLine("To save your result in the win table, please, write your name.");
                        string userName = checkName();

                        string userResult = userName + "|" + DateTime.Now.Date.ToShortDateString() + "|" + (chancesTaken + 1) + "|" + time + "\n";

                        if (!File.Exists(scoreBoardFile))
                        {
                            File.WriteAllText(scoreBoardFile, String.Empty);
                        }
                        File.AppendAllText(scoreBoardFile, userResult);

                        return;
                    }
                }
                else
                {
                    Console.WriteLine("You didn't guess.");

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
            string[] scoreBoard = File.ReadAllLines(scoreBoardFile);

            Console.WriteLine("User's best results:");
            Console.WriteLine();
            Console.WriteLine(("   " + "Name").PadRight(12) + "Guesses".PadRight(8) + "Seconds".PadRight(8));

            string[] personalData = new string[4];
            char[] divider = { '|' };
            userResults.Clear();

            for (int i = 0; i < scoreBoard.Length; i++)
            {
                UserResult userResult = new UserResult();
                personalData = scoreBoard[i].Split(divider);
                userResult.userName = personalData[0];
                userResult.day = personalData[1];
                userResult.wastedChances = Convert.ToInt32(personalData[2]);
                userResult.seconds = Convert.ToInt32(personalData[3]);
                userResults.Add(userResult);
            }

            userResults.Sort();

            for (int i = 0; i < 10 && i < userResults.Count(); i++)
            {
                Console.WriteLine(((i + 1) + ". " + userResults[i].userName).PadRight(12) + userResults[i].wastedChances.ToString().PadRight(8) + userResults[i].seconds);

            }
            Console.WriteLine();
        }





        private void ReadFile(string wordsFile)
        {

            var words = File.ReadAllLines(wordsFile);


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

            Console.WriteLine("Level: " + level);
               
            Console.WriteLine("Your score = " + score);
            Console.WriteLine("You have " + (maxLevelChances - chancesTaken) + " attempts");
            Console.WriteLine("-".PadRight(5) + "1".PadRight(pad) + "2".PadRight(pad) + "3".PadRight(pad) + "4".PadRight(pad));

            for (int i = 0; i < (numberOfCards * 2); i++)
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
                    Console.Write(cards[i].word.PadRight(pad));
                }
                else
                {
                    Console.Write("X".PadRight(pad));
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
                Console.WriteLine("Sorry, you typed in incorrect card. Please, repeate again.");
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
            string letter = coordinate.Substring(0, 1).ToUpper();

            switch (numberOfCards)
            {
                case 4:
                    if ((letter != "A")
                        && (letter != "B"))
                    {
                        result = false;
                    }
                    break;
                case 8:
                    if ((letter != "A")
                        && (letter != "B")
                        && (letter != "C")
                        && (letter != "D"))
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


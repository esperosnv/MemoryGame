using System;
using System.IO;



namespace MemoryGame
{
    class Program
    {

        static int numberOfCards = 8;
        static string[] wordsList = new string[numberOfCards * 2];
        static Card[] cards = new Card[numberOfCards * 2];
        static int padConst = 15;
        static int scores = 0;
        static int chancesTaken = 15;



        static void Main(string[] args)
        {
            Console.WriteLine("Hello User!");
            
            setDate();
            printCards(cards);
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
                //string cardName = new string("card" + (i + 1));
                //Console.WriteLine(cardName);
                Card card = new Card();
                card.word = wordsList[i];
                card.position = i;
                Console.WriteLine(card.word + " " + card.position + " " + card.isOpen);
                cards[i] = card;

            }
        }

        static void printCards(Card[] cards)
        {

            //Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Your scores = " + scores);
            Console.WriteLine("Your have " + chancesTaken + " attemps");
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

                Console.Write(cards[i].word.PadRight(padConst));


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



    }
}
       
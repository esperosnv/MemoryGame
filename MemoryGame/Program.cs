using System;
using System.IO;



namespace consoleApp
{
    class Program
    {

        static int numberOfCards = 8;
        static string[] wordsList = new string[numberOfCards * 2];



        static void Main(string[] args)
        {
            Console.WriteLine("Hello User!");
            ReadFile();
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



            foreach (int i in listNumbers)
            {
                Console.WriteLine(i);
            }

            string[] result = Shuffle(wordsList);
            foreach (string i in result)
            {
                 Console.WriteLine(i + " ");
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


    }
}
       
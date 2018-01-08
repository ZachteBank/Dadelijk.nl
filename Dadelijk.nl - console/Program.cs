using Logic;
using System;

namespace Dadelijk.nl___console
{
    class Program
    {

        private static TaskManagementSystem _tms = new TaskManagementSystem("Data Source=volunteersapp.c153q9deg6j1.us-east-1.rds.amazonaws.com;Initial Catalog=bram;User id=App_bF72Esbab9RD;Password=Gq96h8MhY6JckP9ESScs3SfD;");

        static void Main(string[] args)
        {
            Console.WriteLine("Dadelijk.nl");

            bool run = true;

            while (run)
            {
                Menu();

                ConsoleKeyInfo UserInput = Console.ReadKey(); // Get user input
                int key = -1;

                // We check input for a Digit
                if (char.IsDigit(UserInput.KeyChar))
                {
                    key = int.Parse(UserInput.KeyChar.ToString()); // use Parse if it's a Digit
                }

                switch (key)
                {
                    case 0:
                        AllNewsItems();
                        break;
                    default: case -1:
                        run = false;
                        break;
                        
                }
            }

            End();
        }

        static void Menu()
        {
            Console.Clear();
            Console.WriteLine("Menu\n");
            Console.WriteLine("[0] All newsitems");
        }

        static void AllNewsItems()
        {
            Console.Clear();
            Console.WriteLine("All newsitems\n");
            var newsItems = _tms.AllNewsItems();
            foreach (var newsItem in newsItems)
            {
                Console.WriteLine(newsItem.Subject);
            }
            End();
        }

        static void End()
        {
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
        }
    }
}

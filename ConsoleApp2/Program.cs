using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot(68128, "b6212e15b78cca650d1ced280a7640fc");

            Authenticate(bot).Wait();

            bot.SendMessage("34663453087").Wait();
            bool isRunning = true;
            while (isRunning)
            {
                string[] command = Console.In.ReadLine().Split(' ');
                switch (command[0])
                {
                    case "/help":
                        //TODO help;
                        break;

                    case "/show":
                        //TODO Show;
                        break;

                    case "/quit":
                        //Todo Quit;
                        break;

                    default:
                        Console.WriteLine("Unknown Command " + command);
                        break;
                }
            }
        }

        static async Task Authenticate(Bot bot)
        {
            await bot.Connect();
            await bot.Authenticate("34663453087");
        }


    }
}

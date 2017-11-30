using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot(68128, "b6212e15b78cca650d1ced280a7640fc");

            Authenticate(bot).Wait();

            bool isRunning = true;
            while (isRunning)
            {
                string[] command = Console.In.ReadLine().Split(' ');
                switch (command[0])
                {
                    case "/send":
                        string send = "";
                        for(int i = 0; i < command.Length; i++)
                        {
                            if (i >= 2) send += command[i] + " ";
                        }
                        bot.SendMessage(command[1], command[2]).Wait();
                        break;

                    case "/show":
                        bot.ShowContact(true).Wait();
                        break;

                    case "/quit":
                        if (bot.Stop())
                        {
                            Console.WriteLine("Succesfully Stopped. you can close the console now.");
                        }
                        else
                        {
                            Console.WriteLine("Something Went Wrong.");
                        }
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

﻿using System;
using System.Collections.Generic;
using System.IO;
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
                        for (int i = 0; i < command.Length; i++)
                        {
                            if (i >= 2) send += command[i] + " ";
                        }
                        bot.SendMessage(command[1], send).Wait();
                        break;

                    case "/show":
                        bool exists = command.ElementAtOrDefault(1) != null;
                        if (exists)
                        {
                            if (command[1].Equals("-a") || command[1].Equals("-all") || command[1].Equals("/a") || command[1].Equals("/all")) bot.ShowContact(true).Wait();
                            else bot.ShowContact(false, command[1]).Wait();
                        }
                        else
                        {
                            Console.WriteLine("This command needs parameters");
                        }
                        break;

                    case "/quit":
                        if (bot.Stop())
                        {
                            try
                            {
                                File.Delete(@"./*.dat");
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            finally
                            {
                                Console.WriteLine("Succesfully Stopped. you can close the console now.");
                            }
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

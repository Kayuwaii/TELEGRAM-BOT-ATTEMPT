using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        /*  NOTE:
         *  In this code, we are using MY phone number, it should be changed to YOUR own phone number.
         *  Same goes for API authentiction, this code contains my API ID and API HASH, and therefore should be changed to YOURS.
         */
        static void Main(string[] args)
        {
            Bot bot = new Bot(68128, "b6212e15b78cca650d1ced280a7640fc");
            //Crear un nuevo objeto de tipo Bot. 

            Authenticate(bot).Wait();
            //Authenticate the bot

            bool isRunning = true; //Control the command handling loop.
            while (isRunning)
            {
                bool exists;
                string[] command = Console.In.ReadLine().Split(' ');
                //Split each command word by word

                /*  Command handler:
                 *  Switch statemetn with a case for each different command
                 *  
                 *  Currently there are 4 commands:
                 *      # send: Sends a message to the specified number(if it's on contacts)
                 *      # show: Display Contact info, accepts parameters for either 1 single contact or all of them
                 *      # quit: Stops the bot and deletes session data.
                 *      # sendgroup: The same as send, but for a group.
                 */
                switch (command[0])
                {
                    case "/send":
                        exists = command.ElementAtOrDefault(1) != null; //Check for the command argumnents
                        if (exists)
                        {
                            if (command[1].Equals("-g") || command[1].Equals("-group") || command[1].Equals("/g") || command[1].Equals("/group"))
                            {
                                //Implemented (for now) as a different command
                            }
                            else{
                                string send = "";
                                for (int i = 0; i < command.Length; i++)
                                {
                                    if (i >= 2) send += command[i] + " ";
                                }
                                bot.SendMessage(command[1], send).Wait();
                            }
                        }
                        else
                        {
                            Console.WriteLine("This command needs parameters");
                        }
                        break;

                    case "/show":
                        exists = command.ElementAtOrDefault(1) != null; //Check for the command argumnents
                        if (exists)
                        {
                            if (command[1].Equals("-a") || command[1].Equals("-all") || command[1].Equals("/a") || command[1].Equals("/all")) bot.ShowContact(true).Wait();
                            else bot.ShowContact(false, command[1]).Wait();
                        }
                        else
                        {
                            Console.WriteLine("This command needs parameters");
                        }
                        bot.getGroups();
                        break;

                    case "/quit":
                        if (bot.Stop())
                        {
                            try
                            {
                                File.Delete(@"Session.dat");
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
                    case "/sendgroup":
                        exists = command.ElementAtOrDefault(1) != null; //Check for the command argumnents
                        if (exists)
                        {
                            bot.sendGroup("Test").Wait();
                        }
                        else
                        {
                            Console.WriteLine("This command needs parameters");
                        }
                        break;

                    default:
                        Console.WriteLine("Unknown Command " + command[0]);
                        break;
                }
            }
        }

        /// <summary>
        /// Authenticate to telegram with the specified phone.
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        static async Task Authenticate(Bot bot)
        {
            await bot.Connect();
            await bot.Authenticate("34663453087"); 
        }


    }
}

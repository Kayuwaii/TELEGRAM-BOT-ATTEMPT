using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TLSharp.Core;

namespace ConsoleApp2
{
    class Bot
    {
        private TelegramClient client;

        public Bot(int api_id, string api_hash)
        {
            this.client = new TelegramClient(api_id, api_hash);
        }

        public async Task Connect()
        {
            await this.client.ConnectAsync();
        }

        public async Task Authenticate(String phoneNumber)
        {
            var hash = await client.SendCodeRequestAsync(phoneNumber);

            var code = await Console.In.ReadLineAsync(); // you can change code in debugger

            var user = await client.MakeAuthAsync(phoneNumber, hash, code);
        }

        public async Task SendMessage(string phoneNum)
        {
            //get available contacts
            var result = await client.GetContactsAsync();

            /*foreach (TLUser u in result.Users)
            {
                await Console.Out.WriteLineAsync(u.FirstName);
                await Console.Out.WriteLineAsync(u.Phone);
            }*/
            //find recipient in contacts
            var user = result.Users
                .Where(x => x.GetType() == typeof(TLUser))
                .Cast<TLUser>()
                .FirstOrDefault(x => x.Phone == phoneNum);



            //send message
            await client.SendMessageAsync(new TLInputPeerUser() { UserId = user.Id }, "OUR_MESSAGE");
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Contacts;
using TLSharp.Core;

namespace ConsoleApp2
{
    class Bot
    {
        private TLContacts Contacts;
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

            //get available contacts
            Contacts = await client.GetContactsAsync();
        }

        public async Task SendMessage(string phoneNum)
        {
            //find recipient in contacts
            var user = Contacts.Users
                .Where(x => x.GetType() == typeof(TLUser))
                .Cast<TLUser>()
                .FirstOrDefault(x => x.Phone == phoneNum);
            //send message
            await client.SendMessageAsync(new TLInputPeerUser() { UserId = user.Id }, "OUR_MESSAGE");
        }

        public async Task ShowContact(bool allContacts = false, string phoneNum = null)
        {
            if (allContacts)
            {
                foreach (TLUser u in Contacts.Users)
                {
                    await Console.Out.WriteLineAsync(u.FirstName);
                    await Console.Out.WriteLineAsync(u.Phone);
                }
            }
            else
            {
                if (phoneNum.Length < 11)
                {
                    await Console.Out.WriteLineAsync("No number provided or wrong format. Use /show -all to see all contacts");
                }
                else
                {
                    var user = Contacts.Users
                        .Where(x => x.GetType() == typeof(TLUser))
                        .Cast<TLUser>()
                        .FirstOrDefault(x => x.Phone == phoneNum);
                    await Console.Out.WriteLineAsync(user.FirstName + " " + user.LastName);
                    await Console.Out.WriteLineAsync(user.Phone);
                    await Console.Out.WriteLineAsync((char) user.Id);
                }
            }
        }

    }
}

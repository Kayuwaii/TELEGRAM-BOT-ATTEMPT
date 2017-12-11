using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Contacts;
using TLSharp.Core;

namespace Test
{
    class Bot
    {
        private TLContacts Contacts;
        private TelegramClient client;
        private FileSessionStore store;
        private List<ContactGroup> Groups = new List<ContactGroup>();
        private List<TLUser> tempList = new List<TLUser>();
        public Bot(int api_id, string api_hash)
        {
            store = new FileSessionStore();
            this.client = new TelegramClient(api_id, api_hash, store, "Session");
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

        public async Task SendMessage(string phoneNum, string content = null)
        {
            if (content == null)
            {
                content = "Texto de prueba";
            }
            else
            {
                //find recipient in contacts
                var user = Contacts.Users
                    .Where(x => x.GetType() == typeof(TLUser))
                    .Cast<TLUser>()
                    .FirstOrDefault(x => x.Phone == phoneNum);
                //send message
                await client.SendMessageAsync(new TLInputPeerUser() { UserId = user.Id }, content);
            }
        }

        public void SendMessageGroup(string name, string content = null)
        {
            if (content == null)
            {
                content = "Texto de prueba";
            }
            else
            {
                ContactGroup tempGroup = Groups.Where(x => x.Name.Equals("Test")) as ContactGroup;

                Parallel.ForEach(tempGroup.Members, async (u) =>
                {
                    await client.SendMessageAsync(new TLInputPeerUser() { UserId = u.Id }, content);
                });


            }
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
                    await Console.Out.WriteLineAsync((char)user.Id);
                }
            }
        }

        public void getGroups()
        {
            var tempUser = Contacts.Users
                .Where(x => x.GetType() == typeof(TLUser))
                .Cast<TLUser>()
                .FirstOrDefault(x => x.Phone == "34663453087");
            tempList.Add(tempUser);
            tempUser = Contacts.Users
                .Where(x => x.GetType() == typeof(TLUser))
                .Cast<TLUser>()
                .FirstOrDefault(x => x.Phone == "34635878537");
            tempList.Add(tempUser);
            ContactGroup testGroup = new ContactGroup("Test", tempList, "Es un test premoh");
            Groups.Add(testGroup);
        }

        public async Task sendGroup(string groupName)
        {
            var tempGroup = Groups
                .Where(x => x.GetType() == typeof(ContactGroup))
                .Cast<ContactGroup>()
                .FirstOrDefault(x => x.Name == groupName);

            foreach(TLUser u in tempGroup.Members)
            {
                await SendMessage(u.Phone, "Hola que tal?");
            }
        }

        public bool Stop()
        {
            try
            {
                client.Dispose();

                return true;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
                return false;
            }
        }

    }
}

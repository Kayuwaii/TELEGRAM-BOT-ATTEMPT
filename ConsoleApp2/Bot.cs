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

        /// <summary>
        /// The constructor needs the API ID and API HASH.
        /// Those are obtainable from the my.telegram.com interface.
        /// </summary>
        /// <param name="api_id">API ID</param>
        /// <param name="api_hash">API HASH</param>
        public Bot(int api_id, string api_hash)
        {
            store = new FileSessionStore();
            this.client = new TelegramClient(api_id, api_hash, store, "Session");
            //Create the Client.
        }

        /// <summary>
        /// Establish Connection with the Telegram Service
        /// </summary>
        /// <returns></returns>
        public async Task Connect()
        {
            await this.client.ConnectAsync();
        }

        /// <summary>
        /// This method requests an Authoritzation code to telegram, the user must input then.
        /// </summary>
        /// <param name="phoneNumber">Phone To send the code</param>
        /// <returns></returns>
        public async Task Authenticate(String phoneNumber)
        {
            var hash = await client.SendCodeRequestAsync(phoneNumber);
            //Request Code

            var code = await Console.In.ReadLineAsync();
            //Input code

            var user = await client.MakeAuthAsync(phoneNumber, hash, code);
            //Request Authoritzation with the code


            //get available contacts
            Contacts = await client.GetContactsAsync();

        }

        /// <summary>
        /// Send a message to a specified phone
        /// </summary>
        /// <param name="phoneNum">Reciever phone (in international format)</param>
        /// <param name="content">The content of the message</param>
        /// <returns></returns>
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

        /// <summary>
        /// Send a message to a group
        /// </summary>
        /// <param name="name">Group Name</param>
        /// <param name="content"> Message Content</param>
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

                /*  NOTE:
                 *  groups are not yet implemented to be created through the console,
                 *  but they can be created manually in the code(for now)
                 */
            }
        }

        /// <summary>
        /// Displays the contacts in the clients phone.
        /// </summary>
        /// <param name="allContacts">If true, all contacts will be shown.</param>
        /// <param name="phoneNum">Specify a phone number to view detailed info (International Format)</param>
        /// <returns></returns>
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

        /// <summary>
        /// Temporal method to create a group, and test group capabilities
        /// </summary>
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

        /// <summary>
        /// Send Message to a group.
        /// </summary>
        /// <param name="groupName">group name</param>
        /// <returns></returns>
        public async Task sendGroup(string groupName)
        {
            var tempGroup = Groups
                .Where(x => x.GetType() == typeof(ContactGroup))
                .Cast<ContactGroup>()
                .FirstOrDefault(x => x.Name == groupName);

            foreach (TLUser u in tempGroup.Members)
            {
                await SendMessage(u.Phone, "Hola que tal?");
            }
        }

        //Stop the bot, and dispose of objects.
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;

namespace Test
{
    class ContactGroup
    {
        public string Name;
        public List<TLUser> Members;
        public string Description;

        public ContactGroup(string name, List<TLUser> members, string description)
        {
            Name = name;
            Members = members;
            Description = description;
        }
    }
}

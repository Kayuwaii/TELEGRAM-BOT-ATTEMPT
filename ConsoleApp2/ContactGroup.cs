using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;

namespace Test
{
    /// <summary>
    /// this object will hold a list with users that belong to "x" group (for example a group "teachers" would hold a list with Teachers contacts.
    /// Any contact can be added to a group (as long as it's in contacts)
    /// </summary>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_o3q
    {
        public const short ID = 32428;

        public List<class_i1d> contacts;

        public class_o3q(List<class_i1d> contacts)
        {
            this.contacts = contacts;
        }

        public byte[] write()
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(this.contacts.Count);
            foreach(var contact in this.contacts)
            {
                param1.write(contact.write());
            }
            param1.writeShort(16150);
            return param1.Message.ToArray();
        }
    }
}

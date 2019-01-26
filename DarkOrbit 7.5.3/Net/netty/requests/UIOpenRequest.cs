using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class UIOpenRequest
    {
        public const short ID = 23586;

        public const String ACTION_USER = "user";

        public const String ACTION_SHIP = "ship";

        public const String ACTION_SHIP_WARP = "ship_warp";

        public const String ACTION_CHAT = "chat";

        public const String ACTION_GROUP = "group";

        public const String ACTION_MINIMAP = "minimap";

        public const String ACTION_SPACEMAP = "spacemap";

        public const String ACTION_QUESTS = "quests";

        public const String ACTION_REFINEMENT = "refinement";

        public const String ACTION_LOG = "log";

        public const String ACTION_PET = "pet";

        public const String ACTION_CONTACTS = "contacts";

        public const String ACTION_LOGOUT = "logout";

        public string itemId;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            itemId = parser.readUTF();
        }
    }
}

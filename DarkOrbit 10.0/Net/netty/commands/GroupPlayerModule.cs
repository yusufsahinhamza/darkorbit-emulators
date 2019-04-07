using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupPlayerModule
    {
        public const short ID = 29105;

        public Boolean disconnected = false;
        public GroupPlayerShipModule ship;      
        public FactionModule faction;      
        public String name = "";      
        public GroupPlayerInformationsModule information;      
        public GroupPlayerTargetModule target;      
        public Boolean active = false;      
        public GroupPlayerLocationModule location;      
        public Boolean attacking = false;      
        public int level = 0;      
        public Boolean var51u = false;      
        public Boolean cloaked = false;     
        public GroupPlayerHadesGateModule hades;     
        public int id = 0; 
        public GroupPlayerClanModule clan;

        public GroupPlayerModule(string name, int id, GroupPlayerInformationsModule information, GroupPlayerLocationModule location, int level, bool active, bool cloaked, bool attacking, bool disconnected, bool var51u, GroupPlayerClanModule clan, FactionModule faction, GroupPlayerTargetModule target, GroupPlayerShipModule ship, GroupPlayerHadesGateModule hades)
        {
            this.name = name;
            this.id = id;
            this.information = information;
            this.location = location;
            this.level = level;
            this.active = active;
            this.cloaked = cloaked;
            this.attacking = attacking;
            this.disconnected = disconnected;
            this.var51u = var51u;
            this.clan = clan;
            this.faction = faction;
            this.target = target;
            this.ship = ship;
            this.hades = hades;
        }

        public byte[] write()
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeBoolean(this.disconnected);
            param1.write(ship.write());
            param1.writeShort(19162);
            param1.write(faction.write());
            param1.writeUTF(this.name);
            param1.write(information.write());
            param1.write(target.write());
            param1.writeBoolean(this.active);
            param1.write(location.write());
            param1.writeBoolean(this.attacking);
            param1.writeInt(this.level >> 3 | this.level << 29);
            param1.writeBoolean(this.var51u);
            param1.writeBoolean(this.cloaked);
            param1.write(hades.write());
            param1.writeInt(this.id >> 16 | this.id << 16);
            param1.write(clan.write());
            return param1.Message.ToArray();
        }

        public byte[] writeCommand()
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeBoolean(this.disconnected);
            param1.write(ship.write());
            param1.writeShort(19162);
            param1.write(faction.write());
            param1.writeUTF(this.name);
            param1.write(information.write());
            param1.write(target.write());
            param1.writeBoolean(this.active);
            param1.write(location.write());
            param1.writeBoolean(this.attacking);
            param1.writeInt(this.level >> 3 | this.level << 29);
            param1.writeBoolean(this.var51u);
            param1.writeBoolean(this.cloaked);
            param1.write(hades.write());
            param1.writeInt(this.id >> 16 | this.id << 16);
            param1.write(clan.write());
            return param1.ToByteArray();
        }
    }
}

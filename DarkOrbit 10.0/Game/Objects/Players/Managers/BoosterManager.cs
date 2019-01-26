using Ow.Game.Objects;
using Ow.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Managers
{
    class BoosterManager
    {
        public Player Player { get; set; }

        public List<BoosterTypeModule> DamageTypes = new List<BoosterTypeModule>();
        public List<BoosterTypeModule> ShieldTypes = new List<BoosterTypeModule>();
        public List<BoosterTypeModule> MaxHpTypes = new List<BoosterTypeModule>();

        public BoosterManager(Player player) { Player = player; }

        public void Initiate()
        {
            DamageTypes.Add(new BoosterTypeModule(BoosterTypeModule.DMG_B01));
            ShieldTypes.Add(new BoosterTypeModule(BoosterTypeModule.SHD_B01));
            MaxHpTypes.Add(new BoosterTypeModule(BoosterTypeModule.HP_B01));
            Update();
        }

        public void Add(BoosterType boosterType, int hours)
        {
            Player.SendPacket($"0|A|STM|booster_found|%BOOSTERNAME%|{boosterType.ToString()}|%HOURS%|{hours}");
            switch ((short)boosterType)
            {
                case BoosterTypeModule.DMGM_1:
                case BoosterTypeModule.DMG_B01:
                case BoosterTypeModule.DMG_B02:
                    DamageTypes.Add(new BoosterTypeModule((short)boosterType));
                    break;
                case BoosterTypeModule.SHD_B01:
                case BoosterTypeModule.SHD_B02:
                    ShieldTypes.Add(new BoosterTypeModule((short)boosterType));
                    break;
                case BoosterTypeModule.HP_B01:
                case BoosterTypeModule.HP_B02:
                    MaxHpTypes.Add(new BoosterTypeModule((short)boosterType));
                    break;
            }

            Update();
        }

        public void Remove(BoosterType boosterType)
        {
            switch ((short)boosterType)
            {
                case BoosterTypeModule.DMGM_1:
                case BoosterTypeModule.DMG_B01:
                case BoosterTypeModule.DMG_B02:
                    if(DamageTypes.Count >= 2)
                        DamageTypes.Remove(DamageTypes.FirstOrDefault(x => x.typeValue == (short)boosterType));
                    break;
                case BoosterTypeModule.SHD_B01:
                case BoosterTypeModule.SHD_B02:
                    if (ShieldTypes.Count >= 2)
                        ShieldTypes.Remove(ShieldTypes.FirstOrDefault(x => x.typeValue == (short)boosterType));
                    break;
                case BoosterTypeModule.HP_B01:
                case BoosterTypeModule.HP_B02:
                    if (MaxHpTypes.Count >= 2)
                        MaxHpTypes.Remove(MaxHpTypes.FirstOrDefault(x => x.typeValue == (short)boosterType));
                    break;
            }

            Update();
        }

        public void Update()
        {
            var boostedAttributes = new List<BoosterUpdateModule>();

            if (DamageTypes.Count >= 1)
                boostedAttributes.Add(new BoosterUpdateModule(new BoostedAttributeTypeModule(BoostedAttributeTypeModule.DAMAGE), GetPercentage(BoostedAttributeType.DAMAGE), new List<BoosterTypeModule>())); //DamageTypes -> içerik yazıları için (ama küçük bir sorunu var)
            if (ShieldTypes.Count >= 1)
                boostedAttributes.Add(new BoosterUpdateModule(new BoostedAttributeTypeModule(BoostedAttributeTypeModule.SHIELD), GetPercentage(BoostedAttributeType.SHIELD), new List<BoosterTypeModule>())); //ShieldTypes -> içerik yazıları için (ama küçük bir sorunu var)
            if (MaxHpTypes.Count >= 1)
                boostedAttributes.Add(new BoosterUpdateModule(new BoostedAttributeTypeModule(BoostedAttributeTypeModule.MAXHP), GetPercentage(BoostedAttributeType.MAXHP), new List<BoosterTypeModule>())); //MaxHpTypes -> içerik yazıları için (ama küçük bir sorunu var)

            Player.SendCommand(AttributeBoosterUpdateCommand.write(boostedAttributes));
            Player.SendCommand(AttributeHitpointUpdateCommand.write(Player.CurrentHitPoints, Player.MaxHitPoints, Player.CurrentNanoHull, Player.MaxNanoHull));
            Player.SendCommand(AttributeShieldUpdateCommand.write(Player.CurrentShieldPoints, Player.MaxShieldPoints));
        }

        public int GetPercentage(BoostedAttributeType boostedAttributeType)
        {
            switch (boostedAttributeType)
            {
                case BoostedAttributeType.DAMAGE:
                    return (DamageTypes.Count - 1) * 10;
                case BoostedAttributeType.SHIELD:
                    return (ShieldTypes.Count - 1) * 10;
                case BoostedAttributeType.MAXHP:
                    return (MaxHpTypes.Count - 1) * 10;
                default:
                    return 0;
            }
        }
    }
}

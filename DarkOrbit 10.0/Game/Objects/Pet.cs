using Ow.Game.Movements;
using Ow.Game.Objects.Collectables;
using Ow.Game.Objects.Players;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects
{
    class Pet : Character
    {
        
        public Player Owner { get; set; }

        public override int Speed
        {
            get
            {
                if (Owner != null)
                    return (int)(Owner.Speed * 1.25);
                return 300;
            }
        }

        public bool Activated = false;
        public bool AutoLoot = false;
    
        public Pet(Player player) : base(Randoms.CreateRandomID(), player.Name + "'s P.E.T", player.FactionId, GameManager.GetShip(22), player.Position, player.Spacemap, player.Clan)
        {
            Owner = player;
            MaxHitPoints = Ship.BaseHitpoints;
            MaxShieldPoints = 50000;
            CurrentHitPoints = 50000;
            CurrentShieldPoints = 50000;
            ShieldAbsorption = 0.8;
        }

        public new void Tick()
        {
            if (Activated)
            {
                CheckAutoLoot();
                Movement.ActualPosition(this);
            }
        }

        public void Activate()
        {
            if (!Activated)
            {
                Activated = true;
                Destroyed = false;
                Spacemap = Owner.Spacemap;
                Invisible = Owner.Invisible;
                Position = new Position(Owner.Position.X, Owner.Position.Y);
                Owner.SendPacket("0|A|STM|msg_pet_activated");
                Initialization();
                Spacemap.AddCharacter(this);

                var tickId = -1;
                Program.TickManager.AddTick(this, out tickId);
                TickId = tickId;
            }
            else
            {
                Deactivate();
            }
        }

        public void Deactivate(bool direct = false, bool destroyed = false)
        {
            if (Activated)
            {
                if (LastCombatTime.AddSeconds(10) < DateTime.Now || direct)
                {
                    if (destroyed)
                        Owner.SendPacket("0|A|STM|msg_pet_is_dead");
                    else
                        Owner.SendPacket("0|A|STM|msg_pet_deactivated");

                    Owner.SendPacket("0|PET|D");
                    Activated = false;
                    AutoLoot = false;

                    InRangeCharacters.Clear();
                    Spacemap.RemoveCharacter(this);
                    Program.TickManager.RemoveTick(this);
                }
                else
                {
                    Owner.SendPacket("0|A|STM|msg_pet_in_combat");
                }
            }
        }

        public void Initialization()
        {
            Owner.SendCommand(PetStatusCommand.write(Id, 15, 27000000, 27000000, CurrentHitPoints, MaxHitPoints, CurrentShieldPoints, MaxShieldPoints, 50000, 50000, Speed, Owner.Name + "'s P.E.T."));
            Owner.SendCommand(PetGearAddCommand.write(new PetGearTypeModule(PetGearTypeModule.PASSIVE), 0, 0, true));
            Owner.SendCommand(PetGearAddCommand.write(new PetGearTypeModule(PetGearTypeModule.AUTO_LOOT), 0, 0, true));
            Owner.SendCommand(PetGearSelectCommand.write(new PetGearTypeModule(PetGearTypeModule.PASSIVE), new List<int>()));
            Owner.SendCommand(PetHeroActivationCommand.write(Owner.Id, Id, 22, 3, Owner.Name + "'s P.E.T.", (short)Owner.FactionId, Owner.GetClanId(), 15, Owner.GetClanTag(), Position.X, Position.Y, Speed, new class_11d(class_11d.DEFAULT)));
        }

        public void CheckAutoLoot()
        {
            if (AutoLoot)
            {
                var position = GetNearestCollectable().Position;
                Movement.Move(this, new Position(position.X, position.Y));
            }
            else Follow(Owner);
        }

        private Collectable GetNearestCollectable()
        {
            //yeniden yazılabilir o an bu kadar düşünebildim
            tryAgain:
            var collectablesOrdered =
                Spacemap.Collectables.Values.OrderBy(x => x.Position.DistanceTo(Position));
            var collectable = collectablesOrdered.FirstOrDefault();

            if (!(collectable is BonusBox)) goto tryAgain;

            return collectable;
        }

        private void Follow(Character character)
        {
            var distance = Position.DistanceTo(character.Position);
            if (distance < 450 && character.Moving) return;

            if (character.Moving)
            {
                Movement.Move(this, character.Position);
            }
            else if (Math.Abs(distance - 300) > 250 && !Moving)
                Movement.Move(this, Position.GetPosOnCircle(character.Position, 250));
        }

        public void SwitchGear(short gearId)
        {
            switch (gearId)
            {
                case PetGearTypeModule.PASSIVE:
                    AutoLoot = false;
                    break;
                case PetGearTypeModule.AUTO_LOOT:
                    AutoLoot = true;
                    break;
            }
            Owner.SendCommand(PetGearSelectCommand.write(new PetGearTypeModule(gearId), new List<int>()));
        }
    }
}

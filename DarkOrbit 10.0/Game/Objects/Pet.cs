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
        public bool GuardModeActive = false;
        public short GearId = PetGearTypeModule.PASSIVE;

        public Pet(Player player) : base(Randoms.CreateRandomID(), "P.E.T 15", player.FactionId, GameManager.GetShip(22), player.Position, player.Spacemap, player.Clan)
        {
            Owner = player;
            MaxHitPoints = Ship.BaseHitpoints;
            MaxShieldPoints = 50000;
            CurrentHitPoints = 50000;
            CurrentShieldPoints = 50000;
            ShieldAbsorption = 0.8;
            Damage = 5000;
        }

        public override void Tick()
        {
            if (Activated)
            {
                CheckShieldPointsRepair();
                CheckGuardMode();
                Follow(Owner);
                Movement.ActualPosition(this);
            }
        }

        public DateTime lastShieldRepairTime = new DateTime();
        private void CheckShieldPointsRepair()
        {
            if (LastCombatTime.AddSeconds(10) >= DateTime.Now || lastShieldRepairTime.AddSeconds(1) >= DateTime.Now || CurrentShieldPoints == MaxShieldPoints) return;

            int repairShield = MaxShieldPoints / 25;
            CurrentShieldPoints += repairShield;
            UpdateStatus();

            lastShieldRepairTime = DateTime.Now;
        }

        public DateTime lastAttackTime = new DateTime();
        public DateTime lastRSBAttackTime = new DateTime();
        public void CheckGuardMode()
        {
            if (GuardModeActive)
            {
                foreach (var enemy in Owner.InRangeCharacters.Values)
                {
                    if (Owner.SelectedCharacter != null && Owner.SelectedCharacter != this)
                    {
                        if ((Owner.AttackingOrUnderAttack(5) || Owner.LastAttackTime(5)) || ((enemy is Player && (enemy as Player).LastAttackTime(5)) && enemy.SelectedCharacter == Owner))
                            Attack(Owner.SelectedCharacter);
                    }
                    else
                    {
                        if (((enemy is Player && (enemy as Player).LastAttackTime(5)) && enemy.SelectedCharacter == Owner))
                            Attack(enemy);
                    }
                }
            }
        }

        private void Attack(Character target)
        {
            if (!Owner.AttackManager.TargetDefinition(target, false)) return;
            if ((Owner.Settings.InGameSettings.selectedLaser == AmmunitionManager.RSB_75 ? lastRSBAttackTime : lastAttackTime).AddSeconds(Owner.Settings.InGameSettings.selectedLaser == AmmunitionManager.RSB_75 ? 3 : 1) < DateTime.Now)
            {
                int damageShd = 0, damageHp = 0;

                if (target is Spaceball)
                {
                    var spaceball = target as Spaceball;
                    spaceball.AddDamage(this, Damage);
                }

                double shieldAbsorb = System.Math.Abs(target.ShieldAbsorption - 1);

                if (shieldAbsorb > 1)
                    shieldAbsorb = 1;

                if ((target.CurrentShieldPoints - Damage) >= 0)
                {
                    damageShd = (int)(Damage * shieldAbsorb);
                    damageHp = Damage - damageShd;
                }
                else
                {
                    int newDamage = Damage - target.CurrentShieldPoints;
                    damageShd = target.CurrentShieldPoints;
                    damageHp = (int)(newDamage + (damageShd * shieldAbsorb));
                }

                if ((target.CurrentHitPoints - damageHp) < 0)
                {
                    damageHp = target.CurrentHitPoints;
                }

                if (target is Player && !(target as Player).Attackable())
                {
                    Damage = 0;
                    damageShd = 0;
                    damageHp = 0;
                }

                if (Invisible)
                {
                    Invisible = false;
                    string cloakPacket = "0|n|INV|" + Id + "|0";
                    SendPacketToInRangePlayers(cloakPacket);
                }

                if (target is Player && (target as Player).Storage.Sentinel)
                    damageShd -= Maths.GetPercentage(damageShd, 30);

                var laserRunCommand = AttackLaserRunCommand.write(Id, target.Id, Owner.AttackManager.GetSelectedLaser(), false, false);
                SendCommandToInRangePlayers(laserRunCommand);

                var attackHitCommand =
                        AttackHitCommand.write(new AttackTypeModule(AttackTypeModule.LASER), Id,
                                             target.Id, target.CurrentHitPoints,
                                             target.CurrentShieldPoints, target.CurrentNanoHull,
                                             Damage > damageShd ? Damage : damageShd, false);

                SendCommandToInRangePlayers(attackHitCommand);

                if (damageHp >= target.CurrentHitPoints || target.CurrentHitPoints == 0)
                    target.Destroy(this, DestructionType.PET);
                else
                    target.CurrentHitPoints -= damageHp;

                target.CurrentShieldPoints -= damageShd;
                target.LastCombatTime = DateTime.Now;

                if (Owner.Settings.InGameSettings.selectedLaser == AmmunitionManager.RSB_75)
                    lastRSBAttackTime = DateTime.Now;
                else
                    lastAttackTime = DateTime.Now;

                target.UpdateStatus();
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

                Program.TickManager.AddTick(this, out var tickId);
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
                    {
                        Owner.SendPacket("0|A|STM|msg_pet_is_dead");
                        CurrentHitPoints = 1000;
                        CurrentShieldPoints = 0;
                        UpdateStatus();
                    }
                    else Owner.SendPacket("0|A|STM|msg_pet_deactivated");

                    Owner.SendPacket("0|PET|D");
                    Activated = false;

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

        private void Initialization()
        {
            Owner.SendCommand(PetStatusCommand.write(Id, 15, 27000000, 27000000, CurrentHitPoints, MaxHitPoints, CurrentShieldPoints, MaxShieldPoints, 50000, 50000, Speed, Name));
            Owner.SendCommand(PetGearAddCommand.write(new PetGearTypeModule(PetGearTypeModule.PASSIVE), 0, 0, true));
            Owner.SendCommand(PetGearAddCommand.write(new PetGearTypeModule(PetGearTypeModule.GUARD), 0, 0, true));
            SwitchGear(PetGearTypeModule.PASSIVE);
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
            if (!Activated)
                Activate();

            switch (gearId)
            {
                case PetGearTypeModule.PASSIVE:
                    GuardModeActive = false;
                    break;
                case PetGearTypeModule.GUARD:
                    GuardModeActive = true;
                    break;
            }
            GearId = gearId;
            Owner.SendCommand(PetGearSelectCommand.write(new PetGearTypeModule(gearId), new List<int>()));
        }

        public override byte[] GetShipCreateCommand() { return null; }
    }
}

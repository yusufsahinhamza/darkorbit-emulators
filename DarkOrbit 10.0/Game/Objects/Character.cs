using Ow.Game;
using Ow.Game.Events;
using Ow.Game.Movements;
using Ow.Game.Objects.Collectables;
using Ow.Game.Objects.Players;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ow.Game.Spacemap;

namespace Ow.Game.Objects
{
    abstract class Character : Attackable
    {
        public override string Name { get; set; }
        public override int FactionId { get; set; }
        public override Position Position { get; set; }
        public override Spacemap Spacemap { get; set; }
        public Ship Ship { get; set; }
        public override Clan Clan { get; set; }
        public bool Collecting = false;

        public override int CurrentHitPoints { get; set; }
        public override int MaxHitPoints { get; set; }
        public override int CurrentNanoHull { get; set; }
        public override int MaxNanoHull { get; set; }
        public override int CurrentShieldPoints { get; set; }
        public override int MaxShieldPoints { get; set; }
        public override double ShieldAbsorption { get; set; }
        public override double ShieldPenetration { get; set; }

        public virtual int Speed { get; set; }
        public virtual int Damage { get; set; }
        public virtual int RocketDamage { get; set; }

        public bool Moving { get; set; }
        public Position OldPosition { get; set; }
        public Position Destination { get; set; }
        public Position Direction { get; set; }
        public DateTime MovementStartTime { get; set; }
        public int MovementTime { get; set; }

        public Character SelectedCharacter => Selected as Character;

        protected Character(int id, string name, int factionId, Ship ship, Position position, Spacemap spacemap, Clan clan) : base(id)
        {
            Name = name;
            FactionId = factionId;
            Ship = ship;
            Position = position;
            Spacemap = spacemap;
            Clan = clan;

            Moving = false;
            OldPosition = new Position(0, 0);
            Destination = position;
            Direction = new Position(0, 0);
            MovementStartTime = new DateTime();
            MovementTime = 0;
        }

        public override void Tick()
        {
            if (!Destroyed)
                Tick();
        }

        public void SetPosition(Position targetPosition)
        {
            Destination = targetPosition;
            Position = targetPosition;
            OldPosition = targetPosition;
            Direction = targetPosition;
            Moving = false;

            Movement.Move(this, Movement.ActualPosition(this));
        }

        public event EventHandler<CharacterArgs> InRangeCharacterRemoved;
        public event EventHandler<CharacterArgs> InRangeCharacterAdded;

        public bool AddInRangeCharacter(Character character)
        {
            try
            {
                if (character == null || InRangeCharacters.ContainsKey(character.Id) || character.Destroyed || character.Id == Id || character.Spacemap.Id != Spacemap.Id) return false;

                var success = InRangeCharacters.TryAdd(character.Id, character);

                if (success)
                {
                    InRangeCharacterAdded?.Invoke(this, new CharacterArgs(character));

                    if (this is Player player)
                    {
                        short relationType = character.Clan.Id != 0 && Clan.Id != 0 ? Clan.GetRelation(character.Clan) : (short)0;
                        bool sameClan = Clan.Id != 0 && character.Clan.Id != 0 && Clan == character.Clan;

                        if (character is Player)
                        {
                            var otherPlayer = character as Player;
                            player.SendCommand(otherPlayer.GetShipCreateCommand(player.RankId == 21 ? true : false, relationType, sameClan, (EventManager.JackpotBattle.InActiveEvent(player) && EventManager.JackpotBattle.InActiveEvent(otherPlayer))));

                            if (otherPlayer.Title != "" && (!EventManager.JackpotBattle.InActiveEvent(player) && !EventManager.JackpotBattle.InActiveEvent(otherPlayer)))
                                player.SendPacket($"0|n|t|{otherPlayer.Id}|1|{otherPlayer.Title}");

                            player.SendPacket(otherPlayer.DroneManager.GetDronesPacket());
                            player.SendCommand(DroneFormationChangeCommand.write(otherPlayer.Id, DroneManager.GetSelectedFormationId(otherPlayer.Settings.InGameSettings.selectedFormation)));
                        }
                        else if (character is Pet)
                        {
                            var pet = character as Pet;
                            if (pet == player.Pet) player.SendCommand(PetHeroActivationCommand.write(pet.Owner.Id, pet.Id, 22, 3, pet.Name, (short)pet.Owner.FactionId, pet.Owner.Clan.Id, 15, pet.Owner.Clan.Tag, pet.Position.X, pet.Position.Y, pet.Speed, new class_11d(class_11d.DEFAULT)));
                            else player.SendCommand(PetActivationCommand.write(pet.Owner.Id, pet.Id, 22, 3, pet.Name, (short)pet.Owner.FactionId, pet.Owner.Clan.Id, 15, pet.Owner.Clan.Tag, new ClanRelationModule(relationType), pet.Position.X, pet.Position.Y, pet.Speed, false, true, new class_11d(class_11d.DEFAULT)));
                        }
                        else player.SendCommand(character.GetShipCreateCommand());

                        //player.SendPacket($"0|n|INV|{character.Id}|{Convert.ToInt32(character.Invisible)}");
                        var timeElapsed = (DateTime.Now - character.MovementStartTime).TotalMilliseconds;
                        player.SendCommand(MoveCommand.write(character.Id, character.Destination.X, character.Destination.Y, (int)(character.MovementTime - timeElapsed)));
                    }
                }

                return success;
            }
            catch (Exception e)
            {
                Out.WriteLine("AddInRangeCharacter void exception " + e, "Character.cs");
                return false;
            }
        }

        public bool RemoveInRangeCharacter(Character character)
        {
            try
            {
                if (character.Spacemap != Spacemap || !InRangeCharacters.ContainsKey(character.Id)) return false;

                var success = InRangeCharacters.TryRemove(character.Id, out character);
                if (success)
                {
                    InRangeCharacterRemoved?.Invoke(this, new CharacterArgs(character));

                    if (this is Player player)
                    {
                        if (SelectedCharacter == character)
                            player.Deselection();

                        player.SendCommand(ShipRemoveCommand.write(character.Id));
                    }
                }
                return success;
            }
            catch (Exception e)
            {
                Out.WriteLine("RemoveInRangeCharacter void exception " + e, "Character.cs");
                return false;
            }
        }

        public void Heal(int amount, int healerId = 0, HealType healType = HealType.HEALTH)
        {
            if (amount < 0)
                return;

            switch (healType)
            {
                case HealType.HEALTH:
                    if (CurrentHitPoints + amount > MaxHitPoints)
                        amount = MaxHitPoints - CurrentHitPoints;
                    CurrentHitPoints += amount;
                    break;
                case HealType.SHIELD:
                    if (CurrentShieldPoints + amount > MaxShieldPoints)
                        amount = MaxShieldPoints - CurrentShieldPoints;
                    CurrentShieldPoints += amount;
                    break;
            }

            if (this is Player player)
            {
                var healPacket = "0|A|HL|" + healerId + "|" + Id + "|" + (healType == HealType.HEALTH ? "HPT" : "SHD") + "|" + CurrentHitPoints + "|" + amount;

                if (!Invisible)
                {
                    foreach (var otherPlayers in InRangeCharacters.Values)
                        if (otherPlayers.Selected == this)
                            if (otherPlayers is Player)
                                (otherPlayers as Player).SendPacket(healPacket);
                }

                player.SendPacket(healPacket);
            }

            UpdateStatus();
        }

        public abstract byte[] GetShipCreateCommand();
    }
}

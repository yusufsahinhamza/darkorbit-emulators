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

        public Storage Storage { get; set; }

        public Character SelectedCharacter => Selected as Character;

        protected Character(int id, string name, int factionId, Ship ship, Position position, Spacemap spacemap, Clan clan) : base(id)
        {
            Name = name;
            FactionId = factionId;
            Ship = ship;
            Position = position;
            Spacemap = spacemap;
            Clan = clan;

            Storage = new Storage(this);

            Moving = false;
            OldPosition = new Position(0, 0);
            Destination = position;
            Direction = new Position(0, 0);
            MovementStartTime = new DateTime();
            MovementTime = 0;
        }

        public void RefreshAttackers()
        {
            if (Attackers.Count >= 1)
            {
                foreach (var attacker in Attackers)
                {
                    if (attacker.Value?.Player != null && attacker.Value.LastRefresh.AddSeconds(10) > DateTime.Now)
                    {
                        if (attacker.Value.FadedToGray && MainAttacker == attacker.Value.Player)
                        {
                            attacker.Value.Player.SendPacket($"0|n|USH|{Id}");
                            attacker.Value.FadedToGray = false;
                        }
                        if (!attacker.Value.FadedToGray && MainAttacker != attacker.Value.Player)
                        {
                            attacker.Value.Player.SendPacket($"0|n|LSH|{Id}|{Id}");
                            attacker.Value.FadedToGray = true;
                        }
                        continue;
                    }
                    Attacker removedAttacker;
                    Attackers.TryRemove(attacker.Key, out removedAttacker);
                }
            }
            if (MainAttacker != null)
            {
                if (!Attackers.ContainsKey(MainAttacker.Id))
                {
                    MainAttacker = null;
                }
            }
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

                        if (character is Player)
                        {
                            var otherPlayer = character as Player;
                            player.SendCommand(otherPlayer.GetShipCreateCommand(player, relationType));

                            if (otherPlayer.Title != "" && !EventManager.JackpotBattle.InEvent(otherPlayer))
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

                        var timeElapsed = (DateTime.Now - character.MovementStartTime).TotalMilliseconds;
                        player.SendCommand(MoveCommand.write(character.Id, character.Destination.X, character.Destination.Y, (int)(character.MovementTime - timeElapsed)));
                    }
                }

                return success;
            }
            catch (Exception e)
            {
                Out.WriteLine("AddInRangeCharacter void exception " + e, "Character.cs");
                Logger.Log("error_log", $"- [Character.cs] AddInRangeCharacter void exception: {e}");
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

                    if (Selected == character)
                        Deselection();

                    if (this is Player player)
                        player.SendCommand(ShipRemoveCommand.write(character.Id));
                }
                return success;
            }
            catch (Exception e)
            {
                Out.WriteLine("RemoveInRangeCharacter void exception " + e, "Character.cs");
                Logger.Log("error_log", $"- [Character.cs] RemoveInRangeCharacter void exception: {e}");
                return false;
            }
        }

        public abstract byte[] GetShipCreateCommand();
    }
}

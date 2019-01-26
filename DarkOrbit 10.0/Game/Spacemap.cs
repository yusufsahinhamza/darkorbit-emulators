using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ow.Game.Objects;
using Ow.Game.Objects.Players;
using Ow.Game.Objects.Players.Managers;
using Ow.Game.Movements;
using Ow.Game.Objects.Stations;
using Ow.Managers;
using Ow.Net;
using Ow.Net.netty;
using Ow.Net.netty.commands;
using Ow.Net.netty.handlers;
using Ow.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Game.Objects.Collectables;
using Ow.Game.Objects.Mines;
using System.Collections.Concurrent;
using Ow.Game.Ticks;

namespace Ow.Game
{
    class Spacemap : Tick
    {
        public ConcurrentDictionary<int, Character> Characters = new ConcurrentDictionary<int, Character>();
        public ConcurrentDictionary<int, Activatable> Activatables = new ConcurrentDictionary<int, Activatable>();
        public ConcurrentDictionary<string, Collectable> Collectables = new ConcurrentDictionary<string, Collectable>();
        public ConcurrentDictionary<string, Mine> Mines = new ConcurrentDictionary<string, Mine>();
        public ConcurrentDictionary<string, POI> POIs = new ConcurrentDictionary<string, POI>();

        public int TickId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int FactionId { get; set; }
        public bool StarterMap { get; set; }
        public bool PvpMap { get; set; }
        public string StationsJSON { get; set; }
        public Position[] Limits { get; private set; }

        public int VISIBILITY_RANGE = 2000;

        private List<PortalBase> PortalBase { get; set; }
        private List<StationBase> StationBase { get; set; }

        public Spacemap(int mapID, string name, int factionID, bool starterMap, bool pvpMap, List<PortalBase> portals, List<StationBase> stations)
        {
            Id = mapID;
            Name = name;
            FactionId = factionID;
            StarterMap = starterMap;
            PvpMap = pvpMap;
            PortalBase = portals;
            StationBase = stations;
            ParseLimits();
            LoadObjects();

            if (Id == 101 || Id == 42)
                VISIBILITY_RANGE = 999999999;

            var tickId = -1;
            Program.TickManager.AddTick(this, out tickId);
            TickId = tickId;
        }

        public void Tick()
        {
            foreach (var thisCharacter in Characters.Values)
            {
                foreach (var otherCharacter in Characters.Values)
                {
                    if (!thisCharacter.Equals(otherCharacter))
                    {
                        if (thisCharacter.Position.DistanceTo(otherCharacter.Position) <= VISIBILITY_RANGE)
                        {
                            if (thisCharacter is Player)
                                (thisCharacter as Player).AddInRangeCharacter(otherCharacter);
                            else
                                thisCharacter.InRangeCharacters.TryAdd(otherCharacter.Id, otherCharacter);
                        }
                        else if (((thisCharacter.SelectedCharacter == null || !thisCharacter.SelectedCharacter.Equals(otherCharacter)) &&
                        (otherCharacter.SelectedCharacter == null || !otherCharacter.SelectedCharacter.Equals(thisCharacter))))
                        {
                            if (thisCharacter.IsInRangeCharacter(otherCharacter))
                            {
                                thisCharacter.RemoveInRangeCharacter(otherCharacter);
                            }
                            if (otherCharacter.IsInRangeCharacter(thisCharacter))
                            {
                                otherCharacter.RemoveInRangeCharacter(thisCharacter);
                            }
                        }
                    }
                }
            }
        }

        private void ParseLimits()
        {
            Limits = new Position[] { null, null };
            Limits[0] = new Position(0, 0);
            if (Id == 16 || Id == 29)
                Limits[1] = new Position(41800, 26000);
            else Limits[1] = new Position(20800, 12800);
        }

        public void LoadObjects()
        {
            if (StationBase != null && StationBase.Count >= 1) {
                foreach (var station in StationBase)
                {
                    switch (station.TypeId)
                    {
                        case AssetTypeModule.BASE_COMPANY:
                            var stationPosition = new Position(station.Position[0], station.Position[1]);
                            new HomeStation(this, station.FactionId, stationPosition, null);
                            break;
                    }
                }
            }

            if (PortalBase != null && PortalBase.Count >= 1)
            {
                foreach (var portal in PortalBase)
                {
                    var portalPosition = new Position(portal.Position[0], portal.Position[1]);
                    var targetPosition = new Position(portal.TargetPosition[0], portal.TargetPosition[1]);
                    new Portal(this, portalPosition, targetPosition, portal.TargetSpaceMapId, portal.GraphicId, portal.FactionId, portal.Visible, portal.Working);
                }
            }

            /*
            if (Id == 14)
            {
                var battleStation = new BattleStation(this, 1, new Position(18500, 600), GameManager.GetClan(1));
            }
            */

            if (Id != 101 && Id != 121 && Id != 42)
            {
                for (int i = 0; i <= 85; i++)
                    new BonusBox(AssetTypeModule.BOXTYPE_BONUS_BOX, Position.Random(this, 1000, 19800, 1000, 11800), this, true);

               // for (int i = 0; i <= 15; i++)
               //     new GreenBooty(AssetTypeModule.BOXTYPE_PIRATE_BOOTY, Position.Random(this, 1000, 19800, 1000, 11800), this, true);
            }

            if (Id == 101)
            {
                var poi = new POI("jackpot_poi", POITypes.RADIATION, POIDesigns.SIMPLE, POIShapes.CIRCLE, new List<Position> { new Position(5000, 3200), new Position(1500, 200) }, true, true);
                POIs.TryAdd("jackpot_poi", poi);
            }

            if (Id == 121)
            {
                var poi1 = new POI("uba_poi1", POITypes.NO_ACCESS, POIDesigns.SIMPLE, POIShapes.RECTANGLE, new List<Position> { new Position(5000, 3000), new Position(4000, 4000), new Position(4000, 2000), new Position(6000, 2000) }, true, true);
                var poi2 = new POI("uba_poi2", POITypes.NO_ACCESS, POIDesigns.SIMPLE, POIShapes.CIRCLE, new List<Position> { new Position(4400, 3600), new Position(200, 100) }, true, true);
                var poi3 = new POI("uba_poi3", POITypes.NO_ACCESS, POIDesigns.SIMPLE, POIShapes.CIRCLE, new List<Position> { new Position(5600, 2400), new Position(200, 100) }, true, true);

                POIs.TryAdd("uba_poi1", poi1);
                POIs.TryAdd("uba_poi2", poi2);
                POIs.TryAdd("uba_poi3", poi3);
            }
        }

        public void OnPlayerMovement(Player Player)
        {
            bool inRadiationChanged = CheckRadiation(Player);
            bool assetsChanged = CheckActivatables(Player);
            if (inRadiationChanged || assetsChanged)
                Player.SendCommand(Player.GetBeaconCommand());
        }

        private bool CheckActivatables(Player Player)
        {
            bool isInSecureZone = false;
            bool inEquipZone = false;

            foreach (var entity in Activatables.Values)
            {
                bool inRange = Player.Position.DistanceTo(entity.Position) <= 500;

                short status = inRange ? MapAssetActionAvailableCommand.ON : MapAssetActionAvailableCommand.OFF;

                if (entity is HomeStation)
                {
                    var homeStation = (HomeStation)entity;

                    if (Player.Position.DistanceTo(homeStation.Position) <= HomeStation.SECURE_ZONE_RANGE)
                    {
                        if (homeStation.FactionId == Player.FactionId)
                        {
                            if (!Player.LastAttackTime(3))
                            {
                                isInSecureZone = true;
                                inEquipZone = true;
                            }
                        }
                    }
                }
                else if (entity is RepairStation)
                {
                    if(Player.CurrentHitPoints == Player.MaxHitPoints)
                        inRange = false;
                }

                bool activateButton = Player.UpdateActivatable(entity, inRange);

                if (activateButton)
                {

                    var tooltipItemBars = new List<ClientUITooltipModule>();

                    var class521_localized_1 =
                     new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED);

                    var slotBarItemStatusTooltip_1 =
                    new ClientUITooltipModule(class521_localized_1, ClientUITooltipModule.STANDARD, "q2_condition_JUMP", new List<ClientUITextReplacementModule>());

                    tooltipItemBars.Add(slotBarItemStatusTooltip_1);

                    var assetAction =
                            MapAssetActionAvailableCommand.write(entity.Id,
                                                               status,
                                                               inRange,
                                                               new ClientUITooltipsCommand(
                                                                       entity is Portal ? tooltipItemBars : new List<ClientUITooltipModule>()),
                                                               new class_h45()
                            );
                    Player.SendCommand(assetAction);
                }
            }

            if (Player.IsInEquipZone != inEquipZone)
            {
                Player.IsInEquipZone = inEquipZone;
                Player.SendCommand(EquipReadyCommand.write(inEquipZone));

                if (Player.IsInEquipZone)
                    Player.SendPacket("0|A|STM|msg_equip_ready");
                else
                    Player.SendPacket("0|A|STM|msg_equip_not_ready");
            }

            //save 

            if (Player.IsInDemilitarizedZone != isInSecureZone)
            {
                Player.IsInDemilitarizedZone = isInSecureZone;
                return true;
            }
            return false;
        }

        private bool CheckRadiation(Player Player)
        {
            int positionX = Player.Position.X;
            int positionY = Player.Position.Y;

            bool inRadiationZone = false;

            if (Id == 16 || Id == 42)
                inRadiationZone = positionX < 0 || positionX > 41800 || positionY < 0 || positionY > 26000;
            else
                inRadiationZone = positionX < 0 || positionX > 20900 || positionY < 0 || positionY > 13000;

            foreach (var poi in POIs.Values)
            {
                if (poi.Type == POITypes.RADIATION)
                {
                    if (Player.Position.DistanceTo(poi.ShapeCords[0]) > poi.ShapeCords[1].X)
                        inRadiationZone = true;
                }
            }

            if (Player.IsInRadiationZone != inRadiationZone)
            {
                Player.IsInRadiationZone = inRadiationZone;
                return true;
            }
            return false;
        }

        public void SendObjects(Player player)
        {
            foreach (Activatable activatableStationary in Activatables.Values)
                player.SendCommand(activatableStationary.GetAssetCreateCommand());
            foreach (Collectable collectable in Collectables.Values)
                if (collectable.ToPlayer == null)
                    player.SendCommand(collectable.GetCollectableCreateCommand());
            foreach (var poi in POIs.Values)
                player.SendCommand(poi.GetPOICreateCommand());
            foreach (var mine in Mines.Values)
                player.SendCommand(mine.GetMineCreateCommand());
        }

        public void SendPlayers(Player Player)
        {
            Player.InRangeCharacters.Clear();
            foreach (var character in Characters.Values)
            {
                if (Player.Position.DistanceTo(character.Position) <= VISIBILITY_RANGE)
                {
                    if (Player is Player)
                        (Player as Player).AddInRangeCharacter(character);
                    else
                        Player.InRangeCharacters.TryAdd(character.Id, character);
                }
            }
        }

        public void AddAndInitPlayer(Player player)
        {
            AddCharacter(player);
            LoginRequestHandler.SendSettings(player, false);
            LoginRequestHandler.SendPlayerItems(player);
        }

        public Activatable GetActivatableMapEntity(int pAssetId)
        {
            return !Activatables.ContainsKey(pAssetId) ? null : Activatables[pAssetId];
        }

        public class CharacterArgs : EventArgs
        {
            public Character Character { get; }
            public CharacterArgs(Character character)
            {
                Character = character;
            }
        }

        public event EventHandler<CharacterArgs> PlayerRemoved;
        public event EventHandler<CharacterArgs> PlayerAdded;

        public bool AddCharacter(Character character)
        {
            var success = Characters.TryAdd(character.Id, character);
            if (success)
            {
                PlayerAdded?.Invoke(this, new CharacterArgs(character));
            }
            return success;
        }

        public bool RemoveCharacter(Character character)
        {
            var success = Characters.TryRemove(character.Id, out character);
            if (success)
            {
                PlayerRemoved?.Invoke(this, new CharacterArgs(character));
                foreach (var otherPlayer in Characters.Values)
                {
                    if(otherPlayer is Player)
                        (otherPlayer as Player).RemoveInRangeCharacter(character);
                }
            }
            return success;
        }
    }
}

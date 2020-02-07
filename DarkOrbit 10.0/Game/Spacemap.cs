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
using Ow.Game.Events;

namespace Ow.Game
{
    class OptionsBase
    {
        public bool StarterMap { get; set; }
        public bool PvpMap { get; set; }
        public bool RangeDisabled { get; set; }
        public bool CloakBlocked { get; set; }
        public bool LogoutBlocked { get; set; }
        public bool DeathLocationRepair { get; set; }
    }

    class NpcsBase
    {
        public int ShipId { get; set; }
        public int Amount { get; set; }
    }

    class Spacemap : Tick
    {
        public ConcurrentDictionary<int, Character> Characters = new ConcurrentDictionary<int, Character>();
        public ConcurrentDictionary<int, Activatable> Activatables = new ConcurrentDictionary<int, Activatable>();
        public ConcurrentDictionary<int, Object> Objects = new ConcurrentDictionary<int, Object>();
        public ConcurrentDictionary<string, POI> POIs = new ConcurrentDictionary<string, POI>();

        public int Id { get; set; }
        public string Name { get; set; }
        public int FactionId { get; set; }
        public string StationsJSON { get; set; }
        public Position[] Limits { get; private set; }
        public OptionsBase Options { get; set; }

        private List<NpcsBase> NpcsBase { get; set; }
        private List<PortalBase> PortalBase { get; set; }
        private List<StationBase> StationBase { get; set; }

        public Spacemap(int mapId, string name, int factionId, List<NpcsBase> npcs, List<PortalBase> portals, List<StationBase> stations, OptionsBase options)
        {
            Id = mapId;
            Name = name;
            FactionId = factionId;
            NpcsBase = npcs;
            PortalBase = portals;
            StationBase = stations;
            Options = options;
            ParseLimits();
            LoadObjects();

            Program.TickManager.AddTick(this);
        }

        public void Tick()
        {
            CharacterTicker();
            //ObjectsTicker();
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
                    var position = new Position(station.Position[0], station.Position[1]);

                    switch (station.TypeId)
                    {
                        case AssetTypeModule.BASE_COMPANY:
                            new HomeStation(this, station.FactionId, position, GameManager.GetClan(0));
                            break;
                    }
                }
            }

            if (NpcsBase != null && NpcsBase.Count >= 1)
            {
                foreach (var npc in NpcsBase)
                {
                    for (int i = 1; i <= npc.Amount; i++)
                        new Npc(Randoms.CreateRandomID(), GameManager.GetShip(npc.ShipId), this, Position.Random(this, 0, 20800, 0, 12800));
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
            
            
            if (new int[] { 13, 14, 15 }.Contains(Id))
            {
                for (int i = 1; i <= 125; i++)
                    new BonusBox(Position.Random(this, 0, 20800, 0, 12800), this, true);
                //for (int i = 0; i <= 25; i++)
                    //new GreenBooty(Position.Random(this, 0, 20800, 0, 12800), this, true);
            }

            if (Id == 101)
            {
                var poi = new POI("jackpot_poi", POITypes.RADIATION, POIDesigns.SIMPLE, POIShapes.CIRCLE, new List<Position> { new Position(5000, 3200), new Position(2250, 950) }, true, true);
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

        public void CharacterTicker()
        {
            foreach (var character in Characters.Values)
            {
                foreach (var otherCharacter in Characters.Values.Where(x => x.Id != character.Id && !x.Equals(character)))
                {
                    if (character.InRange(otherCharacter, otherCharacter.RenderRange))
                        character.AddInRangeCharacter(otherCharacter);
                    else if (character.SelectedCharacter == null || (character.SelectedCharacter != null && !character.SelectedCharacter.Equals(otherCharacter)))
                        character.RemoveInRangeCharacter(otherCharacter);
                }

                if (character is Player player)
                {
                    foreach (var obj in Objects.Values)
                    {
                        if (obj is Collectable collectable)
                        {
                            if (!(collectable.ToPlayer == null || (collectable.ToPlayer != null && player == collectable.ToPlayer))) continue;
                        }
                        else if (obj is Mine mine)
                        {
                            if (!(player == mine.Player || !Duel.InDuel(player) || (Duel.InDuel(player) && player.Storage.Duel?.GetOpponent(player) == mine.Player))) continue;
                        }

                        if (player.Position.DistanceTo(obj.Position) <= 1250)
                        {
                            if (!player.Storage.InRangeObjects.ContainsKey(obj.Id))
                            {
                                player.Storage.InRangeObjects.TryAdd(obj.Id, obj);

                                if (obj is Collectable)
                                    player.SendCommand((obj as Collectable).GetCollectableCreateCommand());
                                else if (obj is Mine)
                                    player.SendCommand((obj as Mine).GetMineCreateCommand());
                                else if (obj is Asset)
                                    player.SendCommand((obj as Asset).GetAssetCreateCommand());
                            }
                            else
                            {
                                if (obj is Mine )
                                {
                                    var mine = obj as Mine;
                                    if (mine.Active && mine.activationTime.AddMilliseconds(Mine.ACTIVATION_TIME) < DateTime.Now)
                                    {
                                        if (player.Position.DistanceTo(mine.Position) < Mine.RANGE)
                                        {
                                            mine.Remove();
                                            mine.Explode();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (player.Storage.InRangeObjects.ContainsKey(obj.Id))
                            {
                                player.Storage.InRangeObjects.TryRemove(obj.Id, out var outObj);

                                if (obj is Collectable)
                                    player.SendCommand(DisposeBoxCommand.write((obj as Collectable).Hash, true));
                                else if (obj is Mine)
                                    player.SendPacket($"0|{ServerCommands.REMOVE_ORE}|{(obj as Mine).Hash}");
                                else if (obj is Asset)
                                    player.SendCommand(AssetRemoveCommand.write(new AssetTypeModule((obj as Asset).AssetTypeId), obj.Id));
                            }
                        }
                    }

                    if (CheckRadiation(player) || CheckActivatables(player))
                        player.SendCommand(player.GetBeaconCommand());
                }
            }
        }

        private void ObjectsTicker()
        {
            /*
            foreach (var obj in Objects)
            {
                obj.Value?.Tick();
            }
            */
        }

        public bool CheckActivatables(Player Player)
        {
            bool isInSecureZone = false;
            bool inEquipZone = false;
            bool onBlockedMinePosition = false;

            foreach (var entity in Activatables.Values)
            {
                bool inRange = Player.Position.DistanceTo(entity.Position) <= (entity is HomeStation ? HomeStation.SECURE_ZONE_RANGE : 700);
                short status = inRange ? MapAssetActionAvailableCommand.ON : MapAssetActionAvailableCommand.OFF;

                if (inRange)
                {
                    if (!(entity is BattleStation) && !(entity is Satellite))
                        onBlockedMinePosition = true;

                    if (entity is HomeStation homeStation)
                    {
                        if (homeStation.FactionId == Player.FactionId)
                        {
                            if (!Player.LastAttackTime(5))
                            {
                                isInSecureZone = true;
                                inEquipZone = true;
                            }
                        }
                    }
                    else if (entity is RepairStation)
                    {
                        if (Player.CurrentHitPoints == Player.MaxHitPoints || Player.FactionId != entity.FactionId)
                            inRange = false;
                    }
                }

                bool activateButton = Player.UpdateActivatable(entity, inRange);

                if (activateButton)
                {
                    if (entity is Portal portal && !portal.Working)
                        status = MapAssetActionAvailableCommand.OFF;

                    //TODO check old = if (entity is BattleStation battleStation && battleStation.Clan.Id != 0 && !battleStation.InBuildingState && battleStation.Clan.Id != Player.Clan.Id)

                    if (entity is BattleStation battleStation && battleStation.Clan.Id != 0 && battleStation.Clan.Id != Player.Clan.Id)
                        status = MapAssetActionAvailableCommand.OFF;

                    if (entity is BattleStation && status == MapAssetActionAvailableCommand.OFF)
                    {                      
                        //TODO: Find close ui command end send it
                    }

                    var portalTooltip = new List<ClientUITooltipModule>();
                    portalTooltip.Add(new ClientUITooltipModule(new ClientUITooltipTextFormatModule(ClientUITooltipTextFormatModule.LOCALIZED), ClientUITooltipModule.STANDARD, "q2_condition_JUMP", new List<ClientUITextReplacementModule>()));

                    var assetAction =
                            MapAssetActionAvailableCommand.write(entity.Id,
                                                               status,
                                                               inRange,
                                                               new ClientUITooltipsCommand(entity is Portal ? portalTooltip : new List<ClientUITooltipModule>()),
                                                               new class_h45()
                            );

                    Player.SendCommand(assetAction);
                }
            }

            if (Player.Storage.OnBlockedMinePosition != onBlockedMinePosition)
                Player.Storage.OnBlockedMinePosition = onBlockedMinePosition;

            if (Player.Storage.IsInEquipZone != inEquipZone)
            {
                Player.Storage.IsInEquipZone = inEquipZone;

                Player.SendCommand(EquipReadyCommand.write(Player.Storage.IsInEquipZone));

                var packet = Player.Storage.IsInEquipZone ? "0|A|STM|msg_equip_ready" : "0|A|STM|msg_equip_not_ready";
                Player.SendPacket(packet);
            }

            if (Player.Storage.IsInDemilitarizedZone != isInSecureZone)
            {
                Player.Storage.IsInDemilitarizedZone = isInSecureZone;
                return true;
            }
            return false;
        }

        public bool CheckRadiation(Player Player)
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

            if (Player.Storage.IsInRadiationZone != inRadiationZone)
            {
                Player.Storage.IsInRadiationZone = inRadiationZone;
                return true;
            }
            return false;
        }

        public void SendObjects(Player player)
        {
            foreach (var activatable in Activatables.Values)
            {
                short relationType = player.Clan.Id != 0 && activatable.Clan.Id != 0 ? activatable.Clan.GetRelation(player.Clan) : (short)0;
                player.SendCommand(activatable.GetAssetCreateCommand(relationType));
            }

            foreach (var poi in POIs.Values)
                player.SendCommand(poi.GetPOICreateCommand());

            foreach (var obj in Objects.Values)
                if (obj is Asset asset)
                    player.SendCommand(asset.GetAssetCreateCommand());
        }

        public void AddAndInitPlayer(Player player, bool sendSettings = false)
        {
            var petActivated = player.Pet != null && player.Pet.Activated;

            if (petActivated)
                player.Pet.Deactivate(true);

            LoginRequestHandler.SendPlayer(player);
            AddCharacter(player);

            if (petActivated)
                player.Pet.Activate();

            if (sendSettings)
                LoginRequestHandler.SendSettings(player);
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

        public event EventHandler<CharacterArgs> CharacterRemoved;
        public event EventHandler<CharacterArgs> CharacterAdded;

        public bool AddCharacter(Character character)
        {
            var success = Characters.TryAdd(character.Id, character);
            if (success)
            {
                CharacterAdded?.Invoke(this, new CharacterArgs(character));
            }
            return success;
        }

        public bool RemoveCharacter(Character character)
        {
            var success = Characters.TryRemove(character.Id, out character);
            if (success)
            {
                CharacterRemoved?.Invoke(this, new CharacterArgs(character));

                foreach (var otherCharacter in Characters.Values.Where(x => x.InRangeCharacters.ContainsKey(character.Id)))
                    otherCharacter.RemoveInRangeCharacter(character);
            }
            return success;
        }
    }
}

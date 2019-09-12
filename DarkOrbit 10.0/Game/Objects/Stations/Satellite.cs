using Ow.Game;
using Ow.Game.Movements;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Stations
{
    class Satellite : Activatable
    {
        public int DesignId { get; set; }
        public BattleStation BattleStation { get; set; }
        public StationModuleModule Module { get; set; }
        public int OwnerId { get; set; }

        public override short AssetTypeId => AssetTypeModule.SATELLITE;
        public bool Installed = false;

        public Satellite(BattleStation battleStation, StationModuleModule module, int ownerId, string name, int designId, Position position) : base(battleStation.Spacemap, battleStation.FactionId, position, battleStation.Clan)
        {
            ShieldAbsorption = 0.8;
            BattleStation = battleStation;
            Module = module;
            OwnerId = ownerId;
            Name = name;
            DesignId = designId;

            SetStatus(DesignId);

            Program.TickManager.AddTick(this, out var tickId);
            TickId = tickId;
        }

        public void SetStatus(int designId)
        {
            //check all modules and set different hp and shields whatever
            CurrentHitPoints = 250000;
            MaxHitPoints = 500000;
            CurrentShieldPoints = 250000;
            MaxShieldPoints = 500000;
        }

        public DateTime installationTime = new DateTime();
        public new void Tick()
        {
            if (!Installed)
            {
                if (Module.installationSecondsLeft > 0)
                {
                    var player = GameManager.GetPlayerById(OwnerId);

                    if (player != null && BattleStation != null && player.Position.DistanceTo(BattleStation.Position) < 700)
                    {
                        if (installationTime.AddSeconds(1) < DateTime.Now)
                        {
                            Module.installationSecondsLeft--;
                            installationTime = DateTime.Now;
                        }
                    }
                    else Remove(true);
                }
                else if (Module.installationSecondsLeft <= 0)
                {
                    Installed = true;

                    if (BattleStation.AssetTypeId == AssetTypeModule.BATTLESTATION)
                        RemoveVisualModifier(VisualModifierCommand.BATTLESTATION_INSTALLING);

                    var player = GameManager.GetPlayerById(OwnerId);

                    if (player != null && BattleStation != null)
                        BattleStation.Click(player.GameSession);
                }
            }
            else
            {
                //attack enemies
            }
        }

        public override void Click(GameSession gameSession) { }

        public override byte[] GetAssetCreateCommand(short clanRelationModule = ClanRelationModule.NONE)
        {
            return AssetCreateCommand.write(GetAssetType(), Name,
                                          FactionId, Clan.Tag, Id, DesignId, 0,
                                          Position.X, Position.Y, Clan.Id, false, true, true, true,
                                          new ClanRelationModule(clanRelationModule),
                                          VisualModifiers.Values.ToList());
        }

        public void Remove(bool closeUI = false)
        {
            var player = GameManager.GetPlayerById(OwnerId);

            if (player != null && BattleStation != null)
            {
                BattleStation.EquippedStationModule[player.Clan.Id].Remove(this);

                if (BattleStation.EquippedStationModule[player.Clan.Id].Count == 0)
                    BattleStation.EquippedStationModule.Remove(player.Clan.Id);

                player.Storage.BattleStationModules.Add(Module);

                if (closeUI)
                    player.SendCommand(OutOfBattleStationRangeCommand.write(BattleStation.Id));
            }

            Program.TickManager.RemoveTick(this);
        }
    }
}

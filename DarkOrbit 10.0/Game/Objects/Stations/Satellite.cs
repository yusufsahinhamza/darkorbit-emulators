using Ow.Game.Clans;
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
        private static short ASSET_TYPE = AssetTypeModule.SATELLITE;
        public string Name = "Satellite";
        public int DesignId = 0;

        public Satellite(Spacemap pCurrentSpaceMapId, string name, int designId, int pFactionId, Position position, Clan clan) : base(pCurrentSpaceMapId, pFactionId, position, clan)
        {
            Name = name;
            DesignId = designId;

            foreach (var character in Spacemap.Characters.Values)
            {
                if (character is Player player)
                {
                    short relationType = character.Clan.Id != 0 && Clan.Id != 0 ? Clan.GetRelation(character.Clan) : (short)0;
                    player.SendCommand(GetAssetCreateCommand(relationType));
                }
            }
        }

        public override short GetAssetType() { return ASSET_TYPE; }

        public override void Click(GameSession gameSession)
        {

        }

        public override byte[] GetAssetCreateCommand(short clanRelationModule = ClanRelationModule.NONE)
        {
            return AssetCreateCommand.write(new AssetTypeModule(ASSET_TYPE), Name,
                                          FactionId, Clan.Tag, Id, DesignId, 0,
                                          Position.X, Position.Y, Clan.Id, false, true, true, true,
                                          new ClanRelationModule(clanRelationModule),
                                          new List<VisualModifierCommand>());
        }
    }
}

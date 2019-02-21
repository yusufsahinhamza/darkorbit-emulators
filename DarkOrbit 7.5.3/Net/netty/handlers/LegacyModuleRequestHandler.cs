using Ow.Game;
using Ow.Game.Movements;
using Ow.Game.Objects;
using Ow.Game.Objects.Mines;
using Ow.Game.Objects.Players;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ow.Net.netty.handlers
{
    class LegacyModuleRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new LegacyModuleRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            string[] packet = read.message.Split('|');

            switch (packet[0])
            {
                case ServerCommands.TECHS:
                    player.TechManager.AssembleTechCategoryRequest(packet[1]);
                    break;
                case ClientCommands.PORTAL_JUMP:
                    JumpRequest(gameSession);
                    break;
                case ClientCommands.LASER_STOP:
                    player.DisableAttack(player.SettingsManager.SelectedLaser);
                    break;
                case ServerCommands.ROCKET_ATTACK:
                    player.AttackManager.RocketAttack();
                    break;
                case ServerCommands.SET_STATUS:
                    switch (packet[1])
                    {
                        case ClientCommands.INSTAREPAIR:
                            InstaRepair(gameSession);
                            break;
                        case ClientCommands.CONFIGURATION:
                            player.ChangeConfiguration(packet[2]);
                            break;
                        case ServerCommands.EMP:
                            player.AttackManager.EMP();
                            break;
                        case ServerCommands.INSTASHIELD:
                            player.AttackManager.ISH();
                            break;
                        case ServerCommands.SMARTBOMB:
                            player.AttackManager.SMB();
                            break;
                        case ClientCommands.SELECT_CLOAK:
                            player.CpuManager.Cloak();
                            break;
                        case ClientCommands.AROL:
                            player.CpuManager.ArolX();
                            break;
                        case ClientCommands.RLLB:
                            player.CpuManager.RllbX();
                            break;
                        case ClientCommands.MINE:
                            AssembleMineRequest(packet[2], gameSession);
                            break;
                    }
                    break;
                case ServerCommands.GROUPSYSTEM:
                    switch (packet[1])
                    {
                        case ServerCommands.GROUPSYSTEM_GROUP_INVITE:
                            switch (packet[2])
                            {
                                case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_BY_NAME:
                                    GroupSystem.AssembleInvite(gameSession.Player, GroupSystem.GetPlayerByName(Out.DecodeFrom64(packet[3])));
                                    break;
                                case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_REVOKE:
                                    GroupSystem.Revoke(gameSession.Player, GameManager.GetGameSession(Convert.ToInt32(packet[3]))?.Player);
                                    break;
                                case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ACKNOWLEDGE:
                                    GroupSystem.AssembleAcceptedInvitation(gameSession.Player, GameManager.GetGameSession(Convert.ToInt32(packet[3]))?.Player);
                                    break;
                                case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_REJECT:
                                    GroupSystem.Reject(gameSession.Player, GameManager.GetGameSession(Convert.ToInt32(packet[3]))?.Player);
                                    break;
                            }
                            break;
                        case ServerCommands.GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_KICK:
                            gameSession.Player.Group?.Kick(GameManager.GetGameSession(Convert.ToInt32(packet[2]))?.Player);
                            break;
                        case ServerCommands.GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_LEAVE:
                            gameSession.Player.Group?.Leave(gameSession.Player);
                            break;
                        case ServerCommands.GROUPSYSTEM_BLOCK_INVITATIONS:
                            GroupSystem.BlockInvitations(gameSession.Player);
                            break;
                        case ClientCommands.GROUPSYSTEM_PING:
                            switch (packet[2])
                            {
                                case ClientCommands.GROUPSYSTEM_PING_POSITION:
                                    if (packet.Length < 4)
                                    {
                                        GroupSystem.Ping(gameSession.Player, gameSession.Player.Spacemap.Characters[int.Parse(packet[3])]?.Position);
                                        break;
                                    }
                                    GroupSystem.Ping(gameSession.Player, new Position(int.Parse(packet[3]), int.Parse(packet[4])));
                                    break;
                                case ClientCommands.GROUPSYSTEM_PING_USER:
                                    GroupSystem.Ping(gameSession.Player, GameManager.GetGameSession(Convert.ToInt32(packet[3]))?.Player);
                                    break;
                            }
                            break;
                        case ClientCommands.GROUPSYSTEM_FOLLOW:
                            gameSession.Player.Group?.Follow(gameSession.Player, GameManager.GetGameSession(Convert.ToInt32(packet[2]))?.Player);
                            break;
                        case ClientCommands.GROUPSYSTEM_PROMOTE:
                            gameSession.Player.Group?.ChangeLeader(GameManager.GetGameSession(Convert.ToInt32(packet[2])));
                            break;
                        case ClientCommands.GROUPSYSTEM_SET_REMOTE:
                            switch (packet[2])
                            {
                                case ClientCommands.GROUPSYSTEM_CHANGE_INVITATON_BEHAVIOUR:
                                    gameSession.Player.Group?.ChangeBehavior(gameSession, Convert.ToInt32(packet[3]));
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }

        public void InstaRepair(GameSession gameSession)
        {
            var player = gameSession.Player;

            if (player.CurrentHitPoints != player.MaxHitPoints && !player.AttackingOrUnderAttack() && player.Settings.InGameSettings.inEquipZone)
            {
                int heal = player.MaxHitPoints;
                player.Heal(heal);
                player.SendPacket("0|A|STM|repsuccess");
            }
        }

        public void AssembleMineRequest(string packet, GameSession gameSession)
        {
            var player = gameSession.Player;
            if (player.Storage.IsInDemilitarizedZone || player.CurrentInRangePortalId != -1) return;

            if (player.AttackManager.mineCooldown.AddMilliseconds(TimeManager.MINE_COOLDOWN) < DateTime.Now || player.Storage.GodMode)
            {
                switch (packet)
                {
                    case ServerCommands.MINE_SLM:
                        new SLM_01(player, player.Spacemap, new Position(player.Position.X, player.Position.Y), 7);
                        break;
                    case ServerCommands.MINE_EMP:
                        new EMPM_01(player, player.Spacemap, new Position(player.Position.X, player.Position.Y), 2);
                        break;
                    case ServerCommands.MINE_DDM:
                        new DDM_01(player, player.Spacemap, new Position(player.Position.X, player.Position.Y), 4);
                        break;
                    case ServerCommands.MINE_ACM:
                        new ACM_01(player, player.Spacemap, new Position(player.Position.X, player.Position.Y), 1);
                        break;
                    case ServerCommands.MINE_SAB:
                        new SABM_01(player, player.Spacemap, new Position(player.Position.X, player.Position.Y), 3);
                        break;
                }
                player.SendCooldown(ServerCommands.MINE_COOLDOWN, TimeManager.MINE_COOLDOWN);
                player.AttackManager.mineCooldown = DateTime.Now;
            }
        }

        public void JumpRequest(GameSession gameSession)
        {
            var player = gameSession.Player;

            var activatableStationary = player.Spacemap.GetActivatableMapEntity(player.CurrentInRangePortalId);
            var portalMapEntity = (Portal)player.Spacemap.GetActivatableMapEntity(player.CurrentInRangePortalId);

            if (activatableStationary != null && portalMapEntity != null)
            {
                if (player.Spacemap.PvpMap)
                {
                    if (player.LastCombatTime.AddSeconds(10) < DateTime.Now)
                    {
                        portalMapEntity.Click(gameSession);
                    }
                    else
                    {
                        string jumpError = "0|A|STM|jumpgate_failed_pvp_map";
                        player.SendPacket(jumpError);
                    }
                }
                else
                {
                    portalMapEntity.Click(gameSession);
                }
            }
            else
            {
                String warning = "0|A|STM|jumpgate_failed_no_gate";
                player.SendPacket(warning);
            }
        }
    }
}

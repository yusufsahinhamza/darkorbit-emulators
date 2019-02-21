using Ow.Game.Movements;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Net.netty;
using Ow.Net.netty.commands;
using Ow.Net.netty.handlers;
using Ow.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ow.Game.Objects.Players
{
    class Group : Tick
    {
        public const int LOOT_MODE_RANDOM = 1;
        public const int LOOT_MODE_NEED_BEFORE_GREED = 2;
        public const int LOOT_MODE_WYTIWYG = 3;
        public const int DEFAULT_MAX_GROUP_SIZE = 7;

        public int TickId { get; set; }
        public int Id { get; }
        public Player Leader { get; set; }
        public ConcurrentDictionary<int, Player> Members = new ConcurrentDictionary<int, Player>();
        public bool LeaderInvitesOnly { get; set; }
        public int LootMode { get; set; }

        public Group(Player player, Player acceptedPlayer)
        {
            Id = FindId();
            LootMode = LOOT_MODE_NEED_BEFORE_GREED;

            Leader = player;

            AddToGroup(acceptedPlayer);
            AddToGroup(player);

            DeleteInvitation(player, acceptedPlayer);

            SendInitToAll();

            var tickId = -1;
            Program.TickManager.AddTick(this, out tickId);
            TickId = tickId;
        }

        private int FindId()
        {
            GameManager.Groups.Add(this);
            var id = GameManager.Groups.FindIndex(x => x == this);
            return id;
        }

        private void AddToGroup(Player player)
        {
            Members.TryAdd(player.Id, player);
            player.Group = this;
        }

        public void SendInitToAll()
        {
            foreach (var player in Members)
                InitializeGroup(player.Value);
        }

        private void InitializeGroup(Player player)
        {
            GroupSystem.GroupInitializationCommand(player);
            player.Storage.GroupInitialized = true;
        }

        public DateTime updateTime = new DateTime();
        public void Tick()
        {
            if (Members.Count > 1 && GameManager.Groups.Contains(this))
            {
                if (updateTime.AddSeconds(1) < DateTime.Now)
                {
                    foreach (var groupMemberInstance in Members.Values)
                    {
                        var instance = groupMemberInstance.GameSession;
                        if (instance == null)
                        {
                            Leave(groupMemberInstance);
                            continue;
                        }

                        if (instance.Player.Group == null)
                        {
                            instance.Player.Group = this;
                            SendInitToAll();
                        }

                        if (!instance.Player.Storage.GroupInitialized)
                        {
                            InitializeGroup(instance.Player);
                        }

                        foreach (var _member in Members.Values)
                        {
                            UpdatePlayer(instance, _member);
                        }
                    }

                    updateTime = DateTime.Now;
                }
            }
        }

        private void UpdatePlayer(GameSession instance, Player player)
        {
            if (instance != null && player != null && Members.Count > 1 && Members.ContainsKey(player.Id))
                GroupSystem.UpdateGroup(instance.Player, player, GetStats(player));
        }

        private XElement GetStats(Player player)
        {
            return new XElement("stats",
                new XAttribute("hp", player.CurrentHitPoints),
                new XAttribute("hpM", player.MaxHitPoints),
                new XAttribute("nh", player.CurrentNanoHull),
                new XAttribute("nhM", player.MaxNanoHull),
                new XAttribute("sh", player.CurrentShieldPoints),
                new XAttribute("shM", player.MaxShieldPoints),
                new XAttribute("pos", $"{player.Position.X},{player.Position.Y}"),
                new XAttribute("map", player.Spacemap.Id),
                new XAttribute("lev", player.Level),
                new XAttribute("fra", (int)player.FactionId),
                new XAttribute("act", Convert.ToInt32(true)),
                new XAttribute("clk", Convert.ToInt32(player.Invisible)),
                new XAttribute("shp", Convert.ToInt32(player.Ship.Id)),
                new XAttribute("fgt", Convert.ToInt32(player.AttackManager.Attacking || player.LastCombatTime.AddSeconds(3) > DateTime.Now)),
                new XAttribute("lgo", Convert.ToInt32(GameManager.GetGameSession(player.Id) == null)),
                new XAttribute("tgt", (player.Selected?.Id).ToString()));
        }

        public void Ping(Position position)
        {
            foreach (var member in Members)
                if (member.Value.GameSession != null)
                    member.Value.SendPacket($"0|ps|png|{position.X}|{position.Y}");
        }

        public void Follow(Player player, Player followedPlayer)
        {
            if (player.Spacemap != followedPlayer.Spacemap) return;

            var targetPosition = Movement.ActualPosition(followedPlayer);
            player.SendCommand(HeroMoveCommand.write(targetPosition.X, targetPosition.Y));
            Movement.Move(player, targetPosition);
        }

        public void Accept(Player inviter, Player acceptedPlayer)
        {
            AddToGroup(acceptedPlayer);
            DeleteInvitation(inviter, acceptedPlayer);
            SendInitToAll();
        }

        public void DeleteInvitation(Player inviter, Player player)
        {
            GroupSystem.GroupDeleteInvitationCommand(inviter, player);
            GroupSystem.GroupDeleteInvitationCommand(player, inviter);
        }

        public void Kick(Player player)
        {
            if (player == Leader)
                return;

            player.SendPacket("0|A|STM|msg_grp_leave_group_reason_kick|%name%|" + player.Name);
            Leave(player, true);
        }

        public void ChangeLeader(GameSession leaderSession)
        {
            if (Leader == leaderSession.Player)
                return;

            Leader = leaderSession.Player;
            foreach (var member in Members)
            {
                if (member.Value.GameSession == null) continue;
                member.Value.SendPacket("0|ps|nl|" + Leader.Id);
            }
        }

        public void ChangeBehavior(GameSession gameSession, int newBehaviour)
        {
            if (gameSession.Player != Leader) return;
            LeaderInvitesOnly = Convert.ToBoolean(newBehaviour);
            foreach (var member in Members)
            {
                if (member.Value.GameSession == null) continue;
                member.Value.SendPacket("0|ps|chib|" + Convert.ToInt32(LeaderInvitesOnly));
            }
        }

        public void Destroy()
        {
            Program.TickManager.RemoveTick(this);
            GameManager.Groups.Remove(this);
            foreach (var member in Members)
            {
                member.Value.Group = null;
                member.Value.SendPacket("0|ps|end");
            }
        }

        public void Leave(Player player, bool kicked = false)
        {
            foreach (var member in Members)
            {
                if (member.Value.GameSession == null) continue;
                if (kicked && member.Value.Id == Leader.Id) continue;
                member.Value.SendPacket($"0|{ServerCommands.GROUPSYSTEM}|{ServerCommands.GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES}|{(kicked ? ServerCommands.GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_KICK : ServerCommands.GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_LEAVE)}|{player.Id}");
            }

            if (Members.Count == 2)
            {
                Destroy();
            }
            else
            {
                player.Group = null;
                Members.TryRemove(player.Id, out player);
                if (player != Leader)
                    SendInitToAll();
                else
                    ChangeLeader(Members.FirstOrDefault().Value.GameSession);
            }
        }
    }

    static class GroupSystem
    {
        public static void UpdateGroup(Player player, Player updatedPlayer, XElement xml)
        {
            player.SendPacket("0|ps|upd|" + updatedPlayer.Id + "|" + xml.ToString(SaveOptions.None));
        }

        public static Player GetPlayerByName(string name)
        {
            return GameManager.GameSessions.FirstOrDefault(x => x.Value.Player.Name == name).Value?.Player;
        }

        public static void Ping(Player player, Player pingedUser)
        {
            if (pingedUser == null) return;
            if (pingedUser.Spacemap != player.Spacemap) return;
            player.Group?.Ping(pingedUser.Position);
        }

        public static void Ping(Player player, Position position)
        {
            player.Group?.Ping(position);
        }

        public static void AssembleInvite(Player player, Player invited)
        {
            if (player != invited)
            {
                if (invited == null)
                {
                    player.SendPacket("0|A|STM|msg_grp_inv_err_candidate_nonexistant");
                    return;
                }
                if (invited.Group != null)
                {
                    player.SendPacket("0|A|STM|msg_grp_inv_err_candidate_in_group");
                    return;
                }
                if (invited.Settings.InGameSettings.blockedGroupInvites)
                {
                    player.SendPacket("0|A|STM|msg_grp_inv_err_candidate_blocking");
                    return;
                }
                if (invited.Storage.GroupInvites.ContainsKey(player.Id) && GameManager.Groups.Contains(player.Group))
                {
                    player.SendPacket("0|A|STM|msg_grp_inv_err_duplicate_invitation");
                    return;
                }
                invited.Storage.GroupInvites.Add(player.Id, player.Group);
                GroupInviteCommand(player, invited);
            }
        }

        public static void GroupInviteCommand(Player player, Player invited)
        {
            player.SendPacket("0|ps|inv|new|" + player.Id + "|" + Out.Base64(player.Name) + "|" +
                player.Ship.Id + "|" + invited.Id + "|" +
                Out.Base64(invited.Name) + "|" + invited.Ship.Id);

            invited.SendPacket("0|ps|inv|new|" + player.Id + "|" + Out.Base64(player.Name) + "|" +
                player.Ship.Id + "|" + invited.Id + "|" +
                Out.Base64(invited.Name) + "|" + invited.Ship.Id);
        }

        public static void GroupInitializationCommand(Player player)
        {
            StringBuilder builder =
                new StringBuilder(
                    $"0|ps|init|grp|{player.Group.Id}|{player.Group.Members.Count + 1}|{Group.DEFAULT_MAX_GROUP_SIZE}|{Convert.ToInt32(player.Group.LeaderInvitesOnly)}|{player.Group.LootMode}");
            var groupLeader = player.Group.Leader;

            builder.Append(
                $"|{Out.Base64(groupLeader.Name)}|{groupLeader.Id}|{groupLeader.CurrentHitPoints}|{groupLeader.MaxHitPoints}|{groupLeader.CurrentNanoHull}|{groupLeader.MaxNanoHull}|{groupLeader.CurrentShieldPoints}|{groupLeader.MaxShieldPoints}|{groupLeader.Spacemap.Id}|{groupLeader.Position.X}|{groupLeader.Position.Y}|{groupLeader.Level}|0|{Convert.ToInt32(groupLeader.Invisible)}|{Convert.ToInt32(groupLeader.AttackManager.Attacking)}|{Convert.ToInt32(groupLeader.FactionId)}|{Convert.ToInt32(groupLeader.SelectedCharacter?.Ship.Id)}|{groupLeader.GetClanTag()}|{groupLeader.Ship.Id}|{Convert.ToInt32(GameManager.GetGameSession(groupLeader.Id) == null)}|");

            foreach (var grpMember in player.Group.Members)
            {
                var groupMember = grpMember.Value;
                if (groupMember.Id == player.Group.Leader.Id) continue;

                builder.Append(
                    $"|{Out.Base64(groupMember.Name)}|{groupMember.Id}|{groupMember.CurrentHitPoints}|{groupMember.MaxHitPoints}|{groupMember.CurrentNanoHull}|{groupMember.MaxNanoHull}|{groupMember.CurrentShieldPoints}|{groupMember.MaxShieldPoints}|{groupMember.Spacemap.Id}|{groupMember.Position.X}|{groupMember.Position.Y}|{groupMember.Level}|0|{Convert.ToInt32(groupMember.Invisible)}|{Convert.ToInt32(groupMember.AttackManager.Attacking)}|{Convert.ToInt32(groupMember.FactionId)}|{Convert.ToInt32(groupLeader.SelectedCharacter?.Ship.Id)}|{groupMember.GetClanTag()}|{groupMember.Ship.Id}|{Convert.ToInt32(GameManager.GetGameSession(groupMember.Id) == null)}|");
            }

            player.SendPacket(builder.ToString());
        }

        public static void AssembleAcceptedInvitation(Player player, Player inviter)
        {
            if (inviter == null || !player.Storage.GroupInvites.ContainsKey(inviter.Id))
            {
                player.SendPacket("0|A|STM|msg_grp_inv_err_inviter_nonexistant");
                return;
            }
            if (inviter.Group == null)
            {
                new Group(inviter, player);
            }
            else if (inviter.Group.Members.Count < Group.DEFAULT_MAX_GROUP_SIZE)
            {
                inviter.Group.Accept(inviter, player);
            }
            player.Storage.GroupInvites.Clear();
        }

        public static void BlockInvitations(Player player)
        {
            player.Settings.InGameSettings.blockedGroupInvites = player.Settings.InGameSettings.blockedGroupInvites ? false : true;
            player.SendPacket("0|ps|blk|" + Convert.ToInt32(player.Settings.InGameSettings.blockedGroupInvites));
            QueryManager.SavePlayer.Settings(player);
        }

        public static void Reject(Player player, Player inviter)
        {
            if (player.Storage.GroupInvites.ContainsKey(inviter.Id))
            {
                player.Storage.GroupInvites.Remove(inviter.Id);
                GroupDeleteInvitationCommand(inviter, player, ServerCommands.GROUPSYSTEM_GROUP_INVITATION_DELETE_REJECT);
                GroupDeleteInvitationCommand(player, inviter);
            }
        }

        public static void Revoke(Player player, Player inviter)
        {
            if (player == null) return;
            if (inviter.Storage.GroupInvites.ContainsKey(player.Id))
            {
                inviter.Storage.GroupInvites.Remove(player.Id);

                GroupDeleteInvitationCommand(inviter, player, ServerCommands.GROUPSYSTEM_GROUP_INVITATION_DELETE_REVOKE);
                GroupDeleteInvitationCommand(player, inviter);
            }
        }

        public static void GroupDeleteInvitationCommand(Player player, Player inviter, string reason = ServerCommands.GROUPSYSTEM_GROUP_INVITATION_DELETE_NONE)
        {
            player.SendPacket($"0|{ServerCommands.GROUPSYSTEM}|{ServerCommands.GROUPSYSTEM_GROUP_INVITE}|{ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_DELETE}|{reason}|{inviter.Id}|{player.Id}");
            player.SendPacket($"0|{ServerCommands.GROUPSYSTEM}|{ServerCommands.GROUPSYSTEM_GROUP_INVITE}|{ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_DELETE}|{reason}|{player.Id}|{inviter.Id}");
        }
    }
}



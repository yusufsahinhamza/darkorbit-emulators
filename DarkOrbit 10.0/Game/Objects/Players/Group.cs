using Ow.Game.Movements;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ow.Game.Objects
{
    class Group : Tick
    {
        public const int LOOT_MODE_RANDOM = 1;
        public const int LOOT_MODE_NEED_BEFORE_GREED = 2;
        public const int LOOT_MODE_WYTIWYG = 3;
        public const int DEFAULT_MAX_GROUP_SIZE = 7;

        public int Id { get; }
        public Player Leader { get; set; }
        public ConcurrentDictionary<int, Player> Members = new ConcurrentDictionary<int, Player>();
        public bool LeaderInvitesOnly { get; set; }
        public int LootMode { get; set; }

        public Group(Player player, Player acceptedPlayer)
        {
            try
            {
                Id = FindId();
                LootMode = LOOT_MODE_NEED_BEFORE_GREED;

                Leader = player;

                AddToGroup(acceptedPlayer);
                AddToGroup(player);

                DeleteInvitation(player, acceptedPlayer);

                SendInitToAll();

                Program.TickManager.AddTick(this);
            }
            catch (Exception e)
            {
                Out.WriteLine("Group class exception " + e, "Group.cs");
                Logger.Log("error_log", $"- [Group.cs] Group class exception: {e}");
            }
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
                    }

                    updateTime = DateTime.Now;
                }
            }
        }

        public void UpdateTarget(Player player, List<command_i3O> updates)
        {
            foreach (var member in Members.Values)
            {
                if (member == player) continue;

                if (member != null && player != null && Members.Count > 1 && Members.ContainsKey(player.Id))
                    member.SendCommand(GroupUpdateUICommand.write(player.Id, updates));
            }
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

        public void InitializeGroup(Player player)
        {
            try
            {
                if (player.Group == null) return;

                var groupMembers = new List<GroupPlayerModule>();
                var nonSelected = new GroupPlayerTargetModule(new GroupPlayerShipModule(GroupPlayerShipModule.NONE), "", new GroupPlayerInformationsModule(0, 0, 0, 0, 0, 0));

                foreach (var groupMember in player.Group?.Members.Values)
                {
                    groupMembers.Add(new GroupPlayerModule(groupMember.Name, groupMember.Id, new GroupPlayerInformationsModule(groupMember.CurrentHitPoints, groupMember.MaxHitPoints, groupMember.CurrentShieldPoints, groupMember.MaxShieldPoints, groupMember.CurrentNanoHull, groupMember.MaxNanoHull), new GroupPlayerLocationModule(groupMember.Spacemap.Id, groupMember.Position.X, groupMember.Position.Y), groupMember.Level,
                        true, groupMember.Invisible, groupMember.AttackingOrUnderAttack(), (!GameManager.GameSessions.ContainsKey(groupMember.Id) || groupMember.Destroyed), true, new GroupPlayerClanModule(groupMember.Clan.Id, groupMember.Clan.Tag), new FactionModule((short)groupMember.FactionId), groupMember.Selected == null ? nonSelected : new GroupPlayerTargetModule(new GroupPlayerShipModule(groupMember.SelectedCharacter.Ship.GroupShipId), groupMember.SelectedCharacter.Name,
                        new GroupPlayerInformationsModule(groupMember.Selected.CurrentHitPoints, groupMember.Selected.MaxHitPoints, groupMember.Selected.CurrentShieldPoints, groupMember.Selected.MaxShieldPoints, 0, 0)), new GroupPlayerShipModule(groupMember.Ship.GroupShipId), new GroupPlayerHadesGateModule(false, 0)));
                }

                player.SendCommand(GroupInitializationCommand.write(player.Id, 0, player.Group.Leader.Id, groupMembers, new GroupInvitationBehaviorModule(GroupInvitationBehaviorModule.OFF)));

                player.Storage.GroupInitialized = true;
            }
            catch (Exception e)
            {
                Logger.Log("error_log", $"- [Group.cs] InitializeGroup void exception: {e}");
            }
        }

        public void Ping(Position position)
        {
            foreach (var member in Members)
                if (member.Value.GameSession != null)
                    member.Value.SendCommand(GroupPingCommand.write(position.X, position.Y));
        }

        public void Follow(Player player, Player followedPlayer)
        {
            if (player.Spacemap == followedPlayer.Spacemap)
            {
                var targetPosition = Movement.ActualPosition(followedPlayer);
                player.SendCommand(HeroMoveCommand.write(targetPosition.X, targetPosition.Y));
                Movement.Move(player, targetPosition);
            }
        }

        public void Accept(Player inviter, Player acceptedPlayer)
        {
            AddToGroup(acceptedPlayer);
            DeleteInvitation(inviter, acceptedPlayer);
            SendInitToAll();
        }

        public void DeleteInvitation(Player inviter, Player player)
        {
            inviter.SendCommand(GroupRemoveInvitationCommand.write(inviter.Id, player.Id, GroupRemoveInvitationCommand.NONE));
            player.SendCommand(GroupRemoveInvitationCommand.write(inviter.Id, player.Id, GroupRemoveInvitationCommand.NONE));
        }

        public void Kick(Player player)
        {
            if (player == Leader)
                return;

            player.SendPacket("0|A|STM|msg_grp_leave_group_reason_kick|%name%|" + player.Name);
            Leave(player , true);
        }

        public void ChangeLeader(Player leader)
        {
            if (leader == null || Leader == leader)
                return;

            Leader = leader;
            foreach (var member in Members)
            {
                if (member.Value.GameSession == null) continue;
                member.Value.SendCommand(GroupChangeLeaderCommand.write(Leader.Id));
            }
        }

        public void ChangeBehavior(Player player)
        {
            if (player != Leader) return;
            LeaderInvitesOnly = LeaderInvitesOnly ? false : true;
            foreach (var member in Members)
            {
                if (member.Value.GameSession == null) continue;
                member.Value.SendCommand(GroupUpdateInvitationBehaviorCommand.write(LeaderInvitesOnly ? GroupUpdateInvitationBehaviorCommand.everyone : GroupUpdateInvitationBehaviorCommand.leader));
            }
        }

        public void Destroy()
        {
            Program.TickManager.RemoveTick(this);
            GameManager.Groups.Remove(this);
            foreach (var member in Members)
            {
                member.Value.Group = null;
                member.Value.SendCommand(GroupPlayerLeaveCommand.write(member.Value.Id, GroupPlayerLeaveCommand.LEAVE));
            }
        }

        public void Leave(Player player, bool kicked = false)
        {
            foreach (var member in Members)
            {
                if (member.Value.GameSession == null) continue;
                if (kicked && member.Value.Id == Leader.Id) continue;
                member.Value.SendCommand(GroupPlayerLeaveCommand.write(player.Id, kicked ? GroupPlayerLeaveCommand.KICK : GroupPlayerLeaveCommand.LEAVE));
            }

            if (Members.Count == 2)
                Destroy();
            else
            {
                player.Group = null;
                Members.TryRemove(player.Id, out player);
                if (player != Leader)
                    SendInitToAll();
                else
                    ChangeLeader(Members.FirstOrDefault().Value);
            }
        }
    }
}


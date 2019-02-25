using Ow.Game.Movements;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Net.netty.commands;
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
            /*
            if (instance != null && player != null && Members.Count > 1 && Members.ContainsKey(player.Id))
                instance.Player.SendCommand(GroupUpdateUICommand.write(player.Id, new List<command_i3O> { new GroupPlayerInformationsModule(0, 0, 0, 0, 0, 0) }));

            Console.WriteLine("ok");
            */
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
            try
            {
                var groupMembers = new List<GroupPlayerModule>();
                var nonSelected = new GroupPlayerTargetModule(new GroupPlayerShipModule(GroupPlayerShipModule.NONE), "", new GroupPlayerInformationsModule(0, 0, 0, 0, 0, 0));

                foreach (var grpMember in player.Group.Members)
                {
                    var groupMember = grpMember.Value;

                    groupMembers.Add(new GroupPlayerModule(groupMember.Name, groupMember.Id, new GroupPlayerInformationsModule(groupMember.CurrentHitPoints, groupMember.MaxHitPoints, groupMember.CurrentShieldPoints, groupMember.MaxShieldPoints, groupMember.CurrentNanoHull, groupMember.MaxNanoHull), new GroupPlayerLocationModule(groupMember.Spacemap.Id, groupMember.Position.X, groupMember.Position.Y), groupMember.Level,
                        true, groupMember.Invisible, groupMember.AttackManager.Attacking, !GameManager.GameSessions.ContainsKey(groupMember.Id), true, new GroupPlayerClanModule(groupMember.Clan.Id, groupMember.Clan.Tag), new GroupPlayerFactionModule((short)groupMember.FactionId), groupMember.Selected == null ? nonSelected : new GroupPlayerTargetModule(new GroupPlayerShipModule(GroupPlayerShipModule.SENTINEL), groupMember.SelectedCharacter.Name,
                        new GroupPlayerInformationsModule(groupMember.Selected.CurrentHitPoints, groupMember.Selected.MaxHitPoints, groupMember.Selected.CurrentShieldPoints, groupMember.Selected.MaxShieldPoints, 0, 0)), new GroupPlayerShipModule(GroupPlayerShipModule.SENTINEL), new GroupPlayerHadesGateModule(false, 0)));
                }

                player.SendCommand(GroupInitializationCommand.write(player.Id, 0, player.Group.Leader.Id, groupMembers, new GroupInvitationBehaviorModule(GroupInvitationBehaviorModule.OFF)));

                player.Storage.GroupInitialized = true;
            }
            catch (Exception) { }
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

        public void ChangeLeader(GameSession leaderSession)
        {
            if (Leader == leaderSession.Player)
                return;

            Leader = leaderSession.Player;
            foreach (var member in Members)
            {
                if (member.Value.GameSession == null) continue;
                member.Value.SendCommand(GroupChangeLeaderCommand.write(Leader.Id));
            }
        }

        public void ChangeBehavior(GameSession gameSession)
        {
            if (gameSession.Player != Leader) return;
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
}


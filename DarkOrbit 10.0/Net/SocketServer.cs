using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ow.Chat;
using Ow.Game;
using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static Ow.Game.GameSession;

public class StateObject
{
    public Socket workSocket = null;
    public const int BufferSize = 1024;
    public byte[] buffer = new byte[BufferSize];
    public StringBuilder sb = new StringBuilder();
}

class SocketServer
{
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    public static int Port = 4301;

    public static void StartListening()
    {  
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Port);

        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true)
            {
                allDone.Reset();

                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener);

                allDone.WaitOne();
            }

        }
        catch (Exception e)
        {
            Logger.Log("error_log", $"- [SocketServer.cs] StartListening void exception: {e}");
        }
    }

    public static void AcceptCallback(IAsyncResult ar)
    {
        try
        {
            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            Connection(handler);
        } 
        catch (Exception e)
        {
            Logger.Log("error_log", $"- [SocketServer.cs] AcceptCallback void exception: {e}");
        }
    }

    public static void Connection(Socket handler)
    {
        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
    }

    public static void Execute(JObject json, JObject parameters, Socket handler)
    {
        switch (String(json["Action"]))
        {
            case "OnlineIds":
                Send(handler, JsonConvert.SerializeObject(GameManager.GameSessions.Keys).ToString());
                break;
            case "OnlineCount":
                Send(handler, GameManager.GameSessions.Count.ToString());
                break;
            case "IsOnline":
                var player = GameManager.GetPlayerById(Int(parameters["UserId"]));
                var online = player?.GameSession != null ? true : false;
                Send(handler, online.ToString());
                break;
            case "IsInEquipZone":
                player = GameManager.GetPlayerById(Int(parameters["UserId"]));
                var inEquipZone = player?.GameSession != null ? player.Storage.IsInEquipZone : false;
                Send(handler, inEquipZone.ToString());
                break;
            case "GetPosition":
                player = GameManager.GetPlayerById(Int(parameters["UserId"]));
                var spacemapName = player?.GameSession != null ? player.Spacemap.Name : "";
                Send(handler, spacemapName.ToString());
                break;
            case "AvailableToChangeShip":
                player = GameManager.GetPlayerById(Int(parameters["UserId"]));
                var available = player?.Storage.lastChangeShipTime.AddSeconds(5) < DateTime.Now ? true : false;
                Send(handler, available.ToString());
                break;
            case "BanUser":
                BanUser(GameManager.GetPlayerById(Int(parameters["UserId"])));
                break;
            case "BuyItem":
                BuyItem(GameManager.GetPlayerById(Int(parameters["UserId"])), String(parameters["ItemType"]), (DataType)Short(parameters["DataType"]), Int(parameters["Amount"]));
                break;
            case "ChangeClanData":
                ChangeClanData(GameManager.GetClan(Int(parameters["ClanId"])), String(parameters["Name"]), String(parameters["Tag"]), Int(parameters["FactionId"]));
                break;
            case "ChangeShip":
                ChangeShip(GameManager.GetPlayerById(Int(parameters["UserId"])), GameManager.GetShip(Int(parameters["ShipId"])));
                break;
            case "ChangeCompany":
                ChangeCompany(GameManager.GetPlayerById(Int(parameters["UserId"])), Int(parameters["UridiumPrice"]), Int(parameters["HonorPrice"]));
                break;
            case "UpdateStatus":
                UpdateStatus(GameManager.GetPlayerById(Int(parameters["UserId"])));
                break;
            case "JoinToClan":
                JoinToClan(GameManager.GetPlayerById(Int(parameters["UserId"])), GameManager.GetClan(Int(parameters["ClanId"])));
                break;
            case "LeaveFromClan":
                LeaveFromClan(GameManager.GetPlayerById(Int(parameters["UserId"])));
                break;
            case "CreateClan":
                CreateClan(GameManager.GetPlayerById(Int(parameters["UserId"])), Int(parameters["ClanId"]), Int(parameters["FactionId"]), String(parameters["Name"]), String(parameters["Tag"]));
                break;
            case "DeleteClan":
                DeleteClan(GameManager.GetClan(Int(parameters["ClanId"])));
                break;
            case "StartDiplomacy":
                StartDiplomacy(GameManager.GetClan(Int(parameters["SenderClanId"])), GameManager.GetClan(Int(parameters["TargetClanId"])), Short(parameters["DiplomacyType"]));
                break;
            case "EndDiplomacy":
                EndDiplomacy(GameManager.GetClan(Int(parameters["SenderClanId"])), GameManager.GetClan(Int(parameters["TargetClanId"])));
                break;
            case "UpgradeSkillTree":
                UpgradeSkillTree(GameManager.GetPlayerById(Int(parameters["UserId"])), String(parameters["Skill"]));
                break;
            case "ResetSkillTree":
                ResetSkillTree(GameManager.GetPlayerById(Int(parameters["UserId"])));
                break;
            case "KickPlayer":
                KickPlayer(GameManager.GetPlayerById(Int(parameters["UserId"])), String(parameters["Reason"]));
                break;
        }
    }

    public static void ReadCallback(IAsyncResult ar)
    {
        try
        {
            String content = string.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                content = Encoding.UTF8.GetString(
                    state.buffer, 0, bytesRead);

                if (!string.IsNullOrEmpty(content))
                {
                    var json = Parse(content);
                    var parameters = Parse(json["Parameters"]);

                    Execute(json, parameters, handler);

                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
            }
            else
            {
                Close(handler);
            }
        }
        catch { }
    }

    public static void Close(Socket handler)
    {
        try
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch { }
    }

    private static void Send(Socket handler, String data)
    {
        try
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }
        catch (Exception e)
        {
            Logger.Log("error_log", $"- [SocketServer.cs] Send void exception: {e}");
        }
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket handler = (Socket)ar.AsyncState;

            handler.EndSend(ar);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch (Exception e)
        {
            //Logger.Log("error_log", $"- [SocketServer.cs] SendCallback void exception: {e}");
        }
    }

    public static void KickPlayer(Player player, string reason)
    {
        if (player?.GameSession != null)
        {
            player.SendPacket($"0|A|STD|{reason}");
            player.GameSession.Disconnect(DisconnectionType.NORMAL);
        }
    }

    public static void UpgradeSkillTree(Player player, string skill)
    {
        if (player?.GameSession != null)
        {
            if (skill == "engineering")
                player.SkillTree.engineering++;
            else if (skill == "shieldEngineering")
                player.SkillTree.shieldEngineering++;
            else if (skill == "detonation1")
                player.SkillTree.detonation1++;
            else if (skill == "detonation2")
                player.SkillTree.detonation2++;
            else if (skill == "heatseekingMissiles")
                player.SkillTree.heatseekingMissiles++;
            else if (skill == "rocketFusion")
                player.SkillTree.rocketFusion++;
            else if (skill == "cruelty1")
                player.SkillTree.cruelty1++;
            else if (skill == "cruelty2")
                player.SkillTree.cruelty2++;
            else if (skill == "explosives")
                player.SkillTree.explosives++;
            else if (skill == "luck1")
                player.SkillTree.luck1++;
            else if (skill == "luck2")
                player.SkillTree.luck2++;
        }
    }

    public static void ResetSkillTree(Player player)
    {
        if (player?.GameSession != null)
        {
            player.SkillTree.engineering = 0;
            player.SkillTree.shieldEngineering = 0;
            player.SkillTree.detonation1 = 0;
            player.SkillTree.detonation2 = 0;
            player.SkillTree.heatseekingMissiles = 0;
            player.SkillTree.rocketFusion = 0;
            player.SkillTree.cruelty1 = 0;
            player.SkillTree.cruelty2 = 0;
            player.SkillTree.explosives = 0;
            player.SkillTree.luck1 = 0;
            player.SkillTree.luck2 = 0;
        }
    }

    public static void BanUser(Player player)
    {
        if (player == null) return;

        var client = GameManager.ChatClients[player.Id];
        client.Send($"{ChatConstants.CMD_BANN_USER}%#");
        client.Close();

        player.GameSession.Disconnect(DisconnectionType.NORMAL);
        GameManager.SendChatSystemMessage($"{player.Name} has banned.");
    }

    public static void BuyItem(Player player, string itemType, DataType dataType, int amount)
    {
        if (player?.GameSession != null)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var result = mySqlClient.ExecuteQueryRow($"SELECT data FROM player_accounts WHERE userId = {player.Id}");
                player.Data = JsonConvert.DeserializeObject<DataBase>(result["data"].ToString());
            }

            player.SendPacket($"0|LM|ST|{(dataType == DataType.URIDIUM ? "URI" : "CRE")}|-{amount}|{(dataType == DataType.URIDIUM ? player.Data.uridium : player.Data.credits)}");

            switch (itemType)
            {
                case "drones":
                    player.DroneManager.UpdateDrones(true);
                    break;
                case "booster":
                    var oldBoosters = player.BoosterManager.Boosters;

                    using (var mySqlClient = SqlDatabaseManager.GetClient())
                    {
                        var result = mySqlClient.ExecuteQueryRow($"SELECT boosters FROM player_equipment WHERE userId = {player.Id}");
                        var newBoosters = JsonConvert.DeserializeObject<Dictionary<short, List<BoosterBase>>>(result["boosters"].ToString());
                        player.BoosterManager.Boosters = newBoosters.Concat(oldBoosters).GroupBy(b => b.Key).ToDictionary(b => b.Key, b => b.First().Value);
                    }

                    player.BoosterManager.Update();
                    break;
            }
        }
    }

    public static void ChangeClanData(Clan clan, string name, string tag, int factionId)
    {
        if (clan.Id != 0)
        {
            clan.Tag = tag;
            clan.Name = name;
            //clan.FactionId = factionId;

            foreach (GameSession gameSession in GameManager.GameSessions.Values.Where(x => x.Player.Clan.Id == clan.Id))
            {
                var player = gameSession.Player;
                if (player != null)
                    GameManager.SendCommandToMap(player.Spacemap.Id, ClanChangedCommand.write(clan.Tag, clan.Id, player.Id));
            }
        }
    }

    public static void JoinToClan(Player player, Clan clan)
    {
        if (player?.GameSession != null && clan != null)
        {
            player.Clan = clan;

            var command = ClanChangedCommand.write(clan.Tag, clan.Id, player.Id);
            player.SendCommand(command);
            player.SendCommandToInRangePlayers(command);
        }
    }

    public static void EndDiplomacy(Clan senderClan, Clan targetClan)
    {
        if (senderClan != null && targetClan != null)
        {
            senderClan.Diplomacies.Remove(targetClan.Id);
            targetClan.Diplomacies.Remove(senderClan.Id);
        }
    }

    public static void StartDiplomacy(Clan senderClan, Clan targetClan, short diplomacyType)
    {
        if (senderClan != null && targetClan != null)
        {
            senderClan.Diplomacies.Add(targetClan.Id, (Diplomacy)diplomacyType);
            targetClan.Diplomacies.Add(senderClan.Id, (Diplomacy)diplomacyType);
        }
    }

    public static void LeaveFromClan(Player player)
    {
        foreach (var battleStation in GameManager.BattleStations.Values)
        {
            if (battleStation.EquippedStationModule.ContainsKey(player.Clan.Id))
                battleStation.EquippedStationModule[player.Clan.Id].ForEach(x => { if (x.OwnerId == player.Id) { x.Destroy(null, DestructionType.MISC); } });
        }

        if (player?.GameSession != null)
        {
            if (player.Clan.Id != 0)
            {
                player.Clan = GameManager.GetClan(0);

                var command = ClanChangedCommand.write(player.Clan.Tag, player.Clan.Id, player.Id);
                player.SendCommand(command);
                player.SendCommandToInRangePlayers(command);
            }
        }
    }

    public static void DeleteClan(Clan deletedClan)
    {
        if (deletedClan != null)
        {
            foreach (var battleStation in GameManager.BattleStations.Values.Where(x => x.Clan.Id == deletedClan.Id))
                battleStation.Destroy(null, DestructionType.MISC);

            GameManager.Clans.TryRemove(deletedClan.Id, out deletedClan);

            foreach (var gameSession in GameManager.GameSessions.Values)
            {
                var member = gameSession?.Player;

                if (member != null && member.Clan.Id == deletedClan.Id)
                {
                    member.Clan = GameManager.GetClan(0);

                    var command = ClanChangedCommand.write(member.Clan.Tag, member.Clan.Id, member.Id);
                    member.SendCommand(command);
                    member.SendCommandToInRangePlayers(command);
                }
            }

            foreach (var clan in GameManager.Clans.Values)
                clan.Diplomacies.Remove(deletedClan.Id);
        }
    }

    public static void CreateClan(Player player, int clanId, int factionId, string name, string tag)
    {
        if (!GameManager.Clans.ContainsKey(clanId))
        {
            var clan = new Clan(clanId, name, tag, factionId);
            GameManager.Clans.TryAdd(clan.Id, clan);

            if (player?.GameSession != null)
            {
                player.Clan = clan;

                var command = ClanChangedCommand.write(clan.Tag, clan.Id, player.Id);
                player.SendCommand(command);
                player.SendCommandToInRangePlayers(command);
            }
        }
    }

    public static void ChangeCompany(Player player, int uridiumPrice, int honorPrice)
    {
        if (player?.GameSession != null)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var result = mySqlClient.ExecuteQueryRow($"SELECT data, factionId FROM player_accounts WHERE userId = {player.Id}");
                player.Data = JsonConvert.DeserializeObject<DataBase>(result["data"].ToString());
                player.FactionId = Convert.ToInt32(result["factionId"]);
            }

            player.SendPacket($"0|LM|ST|URI|-{uridiumPrice}|{player.Data.uridium}");

            if (honorPrice > 0)
                player.SendPacket($"0|LM|ST|HON|-{honorPrice}|{player.Data.honor}");

            player.Jump(player.GetBaseMapId(), player.GetBasePosition());
        }
    }

    public static void ChangeShip(Player player, Ship ship)
    {
        if (player?.GameSession != null && ship != null)
        {
            player.ChangeShip(ship.Id);
            player.Storage.lastChangeShipTime = DateTime.Now;
        }
    }

    public static void UpdateStatus(Player player)
    {
        if (player?.GameSession != null)
        {
            QueryManager.SetEquipment(player);   

            player.DroneManager.UpdateDrones(true);
            player.UpdateStatus();
        }
    }

    public static int Int(object value)
    {
        try
        {
            return Convert.ToInt32(value.ToString());

        }
        catch (Exception e)
        {
            return 0;
        }
    }

    public static short Short(object value)
    {
        try
        {
            return Convert.ToInt16(value.ToString());

        }
        catch (Exception e)
        {
            return 0;
        }
        
    }

    public static string String(object value)
    {
        try
        {
            return value.ToString();

        }
        catch (Exception e)
        {
            return "";
        }
        
    }

    public static JObject Parse(object value)
    {
        try
        {
            return JObject.Parse(value.ToString());

        }
        catch (Exception e)
        {
            return null;
        }
       
    }
}
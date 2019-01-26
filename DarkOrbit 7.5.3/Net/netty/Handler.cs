using Ow.Game;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.handlers;
using Ow.Net.netty.requests;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty
{
    class Handler
    {
        private static Dictionary<short, IHandler> Commands = new Dictionary<short, IHandler>();

        public static void AddCommands()
        {
            Commands.Add(AudioSettingsRequest.ID, new AudioSettingsRequestHandler());
            Commands.Add(QualitySettingsRequest.ID, new QualitySettingsRequestHandler());
            Commands.Add(DisplaySettingsRequest.ID, new DisplaySettingsRequestHandler());
            Commands.Add(WindowSettingsRequest.ID, new WindowSettingsRequestHandler());
            Commands.Add(GameplaySettingsRequest.ID, new GameplaySettingsRequestHandler());
            Commands.Add(ShipSettingsRequest.ID, new ShipSettingsRequestHandler());
            Commands.Add(UserKeyBindingsUpdateRequest.ID, new UserKeyBindingsUpdateHandler());
            Commands.Add(LegacyModule.ID, new LegacyModuleRequestHandler());
            Commands.Add(MoveRequest.ID, new MoveRequestHandler());
            Commands.Add(ShipSelectRequest.ID, new ShipSelectRequestHandler());
            Commands.Add(DroneFormationChangeRequest.ID, new DroneFormationChangeRequestHandler());
            Commands.Add(SelectBatteryRequest.ID, new SelectBatteryRequestHandler());
            Commands.Add(SelectRocketRequest.ID, new SelectRocketRequestHandler());
            Commands.Add(HellstormSelectRocketRequest.ID, new HellstormSelectRocketRequestHandler());
            Commands.Add(KillscreenRequest.ID, new KillsceenRequestHandler());
            Commands.Add(CollectBoxRequest.ID, new CollectBoxRequestHandler());
            Commands.Add(AbilityLaunchRequest.ID, new AbilityLaunchRequestHandler());
            Commands.Add(PetRequest.ID, new PetRequestHandler());
            Commands.Add(PetGearActivationRequest.ID, new PetGearActivationRequestHandler());
            Commands.Add(ShipWarpRequest.ID, new ShipWarpRequestHandler());
            Commands.Add(29918, new AttackLaserRequestHandler());
            Commands.Add(31394, new HellstormLaunchRequestHandler());
            Commands.Add(11209, new HellstormLoadRequestHandler());
            Commands.Add(5239, new ShipWarpWindowRequestHandler());
            Commands.Add(LogoutRequest.ID, new LogoutRequestHandler());
            /*         
           
            Commands.Add(AssetHandleClickRequest.ID, new AssetHandleClickHandler());
          
           
            
  
            */
        }

        public static void Execute(byte[] bytes, GameClient client)
        {
            var parser = new ByteParser(bytes);

            if (parser.CMD_ID == VersionRequest.ID)
            {
                var read = new VersionRequest();
                read.readCommand(bytes);

                if (QueryManager.CheckSessionId(read.major, read.minor))
                    new VersionRequestHandler(client, read.major);

                return;
            }

            var gameSession = GameManager.GetGameSession(client.UserId);
            if (gameSession == null) return;

            if (Commands.ContainsKey(parser.CMD_ID))
            {
                Commands[parser.CMD_ID].execute(gameSession, bytes);
                gameSession.LastActiveTime = DateTime.Now;
            }
            else
                Out.WriteLine("Unknown command ID: " + parser.CMD_ID);
        }
    }
}

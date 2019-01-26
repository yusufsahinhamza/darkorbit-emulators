using Ow.Game;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ow.Game.Objects.Players.Managers.PlayerSettings;

namespace Ow.Net.netty.handlers
{
    class SlotBarConfigSetRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new SlotBarConfigSetRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var settings = player.SettingsManager;

            if (read.FromIndex != 0)
            {
                switch (read.FromSlotBarId)
                {
                    case SettingsManager.STANDARD_SLOT_BAR:
                        settings.SlotBarItems.Remove((short)read.FromIndex);

                        var standart = player.Settings.SlotBarItems;
                        standart[read.FromIndex - 1] = new SlotBarItemsBase("");
                        break;
                    case SettingsManager.PREMIUM_SLOT_BAR:
                        settings.PremiumSlotBarItems.Remove((short)read.FromIndex);

                        var premium = player.Settings.PremiumSlotBarItems;
                        premium[read.FromIndex - 1] = new SlotBarItemsBase("");
                        break;
                    case SettingsManager.PRO_ACTION_BAR:
                        settings.ProActionBarItems.Remove((short)read.FromIndex);

                        var proAction = player.Settings.ProActionBarItems;
                        proAction[read.FromIndex - 1] = new SlotBarItemsBase("");
                        break;
                }
            }
            if (read.ToIndex != 0)
            {
                switch (read.ToSlotBarId)
                {
                    case SettingsManager.STANDARD_SLOT_BAR:
                        settings.SlotBarItems.Remove((short)read.ToIndex);
                        settings.SlotBarItems.Add((short)read.ToIndex, read.ItemId);

                        var standart = player.Settings.SlotBarItems;
                        standart[read.ToIndex - 1] = new SlotBarItemsBase(read.ItemId);
                        break;
                    case SettingsManager.PREMIUM_SLOT_BAR:
                        settings.PremiumSlotBarItems.Remove((short)read.ToIndex);
                        settings.PremiumSlotBarItems.Add((short)read.ToIndex, read.ItemId);

                        var premium = player.Settings.PremiumSlotBarItems;
                        premium[read.ToIndex - 1] = new SlotBarItemsBase(read.ItemId);
                        break;
                    case SettingsManager.PRO_ACTION_BAR:
                        settings.ProActionBarItems.Remove((short)read.ToIndex);
                        settings.ProActionBarItems.Add((short)read.ToIndex, read.ItemId);

                        var proAction = player.Settings.ProActionBarItems;
                        proAction[read.ToIndex - 1] = new SlotBarItemsBase(read.ItemId);
                        break;
                }
            }

            QueryManager.SavePlayer.Settings(player);
            settings.SendSlotBarCommand();
            player.SendCurrentCooldowns();
        }
    }
}

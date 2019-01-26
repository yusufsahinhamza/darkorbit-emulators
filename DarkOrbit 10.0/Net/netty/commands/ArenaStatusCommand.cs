using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ArenaStatusCommand
    {
        public const short ID = 31290;

        public const short JACKPOT = 0;
        public const short SCHEDULED = 0;
        public const short WAITING_FOR_PLAYERS = 1;
        public const short COUNTDOWN = 2;
        public const short FIGHTING = 3;
        public const short RADIATION_ACTIVE = 4;
        public const short DONE = 5;
        public const short DESTROYABLE = 6;
        public const short NONE = 7;

        public static byte[] write(short arenaType, short status, int currentRound, int survivors, int participants, String opponentName, int opponentId, int opponentInstanceId, int secondsLeftInPhase, int warpWarningOffsetSeconds)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(warpWarningOffsetSeconds >> 7 | warpWarningOffsetSeconds << 25);
            param1.writeShort(status);
            param1.writeInt(currentRound >> 8 | currentRound << 24);
            param1.writeShort(24150);
            param1.writeInt(opponentId << 16 | opponentId >> 16);
            param1.writeUTF(opponentName);
            param1.writeInt(participants >> 12 | participants << 20);
            param1.writeInt(survivors << 6 | survivors >> 26);
            param1.writeInt(opponentInstanceId >> 10 | opponentInstanceId << 22);
            param1.writeInt(secondsLeftInPhase << 15 | secondsLeftInPhase >> 17);
            param1.writeShort(arenaType);
            return param1.ToByteArray();
        }
    }
}

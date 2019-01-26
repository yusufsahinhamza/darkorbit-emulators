using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Utils
{
    class ByteArray
    {
        public List<byte> Message;
        public bool NROL;

        public ByteArray(short ID, bool NeedReverseOrLength = true)
        {
            Message = new List<byte>();
            NROL = NeedReverseOrLength;
            writeShort(ID);
        }

        public void writeInt(int Int32)
        {
            write(BitConverter.GetBytes(Int32), true);
        }

        public void writeShort(short Short)
        {
            write(BitConverter.GetBytes(Short), true);
        }
        public void writeDouble(double num)
        {
            write(BitConverter.GetBytes(num), true);
        }

        public void writeFloat(float num)
        {
            write(BitConverter.GetBytes(num), true);
        }
        public void writeUTF(string String)
        {
            writeShort((short)String.Length);
            write(Encoding.Default.GetBytes(String), false);
        }

        public void writeBoolean(bool Bool)
        {
            write(new byte[] { (Bool) ? (byte)1 : (byte)0 }, false);
        }

        public void write(byte[] Bytes, bool IsInt = false)
        {
            if (IsInt)
            {
                for (int i = Bytes.Length - 1; i > -1; i--)
                {
                    this.Message.Add(Bytes[i]);
                }
            }
            else
            {
                this.Message.AddRange(Bytes);
            }
        }

        public byte[] ToByteArray()
        {

            List<byte> NewMsg = new List<byte>();
            NewMsg.AddRange(BitConverter.GetBytes(Message.Count));
            NewMsg.Reverse();
            NewMsg.AddRange(Message);

            return NewMsg.ToArray();
        }
    }

    class ByteReader
    {
        public static int ReadInt(byte[] data)
        {
            int outputResult = 0;
            outputResult += data[0] << 24;
            outputResult += data[1] << 16;
            outputResult += data[2] << 8;
            outputResult += data[3];
            return outputResult;
        }

        public static int ReadShort(byte[] data)
        {
            int outputResult = 0;
            outputResult += data[0] << 8;
            outputResult += data[1];
            return outputResult;
        }
    }

    class ByteParser
    {
        private byte[] Body;
        private int Pointer = 0;

        public ByteParser(byte[] Packet)
        {
            Body = Packet;
            this.Lenght = readShort();
            this.CMD_ID = readShort();
        }

        public short Lenght;
        public short CMD_ID;

        public short readShort()
        {
            short value = BitConverter.ToInt16(new byte[] { Body[Pointer + 1], Body[Pointer] }, 0);
            Pointer += 2;

            return value;
        }

        public int readInt()
        {
            int value = BitConverter.ToInt32(new byte[] { Body[Pointer + 3], Body[Pointer + 2], Body[Pointer + 1], Body[Pointer] }, 0);
            Pointer += 4;

            return value;
        }

        public string readUTF()
        {
            try
            {
                int Lenght = readShort();
                string data = Encoding.UTF8.GetString(Body, Pointer, Lenght);
                Pointer += Lenght;
                return data;
            }
            catch (System.Exception)
            {
                return "";
            }
        }

        public bool readBoolean()
        {
            return Body[Pointer++] == 1;
        }
    }
}

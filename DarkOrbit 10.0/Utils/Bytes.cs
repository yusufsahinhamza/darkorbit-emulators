using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Utils
{
    class ByteArray
    {
        //Credits to Yuuki and E*PVP

        public List<byte> Message;
        public bool NROL;

        public ByteArray(short ID, bool NeedReverseOrLength = true)
        {
            Message = new List<byte>();
            NROL = NeedReverseOrLength;
            writeShort(ID);
        }

        public ByteArray(bool NeedReverseOrLenght = false)
        {
            Message = new List<byte>();
            NROL = NeedReverseOrLenght;
        }

        public void setNROL(bool to)
        {
            NROL = to;
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

        public void writeLong(long num)
        {
            write(BitConverter.GetBytes(num));
        }

        public void writeFloat(float num)
        {
            write(BitConverter.GetBytes(num), true);
        }
        public void writeUTF(string String)
        {
            writeShort((short)Encoding.UTF8.GetByteCount(String));
            write(Encoding.UTF8.GetBytes(String), false);
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
            NewMsg.AddRange(BitConverter.GetBytes((short)Message.Count));
            NewMsg.Reverse();
            NewMsg.AddRange(Message);

            return NewMsg.ToArray();
        }
    }

    class ByteParser
    {
        //Credits to E*PVP

        public byte[] Array { get; set; }
        public int Counter = 0;
        public short Lenght;
        public short ID;

        public ByteParser(byte[] Array)
        {
            this.Array = Array;

            this.Lenght = readShort();
            this.ID = readShort();
        }

        private int ReadShort(byte[] data)
        {
            int outputResult = 0;
            outputResult += data[0] << 8;
            outputResult += data[1];
            return outputResult;
        }

        public short readShort()
        {
            return (short)Convert.ToInt32(ReadShort(new byte[] { Array[Counter++], Array[Counter++] }));
        }

        private int ReadInt(byte[] data)
        {
            int outputResult = 0;
            outputResult += data[0] << 24;
            outputResult += data[1] << 16;
            outputResult += data[2] << 8;
            outputResult += data[3];
            return outputResult;
        }

        public int readInt()
        {
            return ReadInt(new byte[] { Array[Counter++], Array[Counter++], Array[Counter++], Array[Counter++], });
        }

        public long readLong()
        {
            long value = BitConverter.ToInt64(new byte[] { Array[Counter + 7], Array[Counter + 6], Array[Counter + 5], Array[Counter + 4], Array[Counter + 3], Array[Counter + 2], Array[Counter + 1], Array[Counter] }, 0);
            Counter += 8;

            return value;
        }

        public double readDouble()
        {
            return BitConverter.Int64BitsToDouble(readLong());
        }

        public double readFloat()
        {
            var value = BitConverter.ToSingle(new byte[] { Array[Counter + 3], Array[Counter + 2], Array[Counter + 1], Array[Counter] }, 0);
            Counter += 4;
            return value;
        }

        public string readUTF()
        {
            try
            {
                short stringLength = readShort();
                string value = Encoding.UTF8.GetString(Array, Counter, stringLength);

                Counter += stringLength;

                return value;
            }
            catch (Exception) { return ""; }
        }

        public bool readBoolean()
        {
            return Array[Counter++] == 1;
        }
    }
}

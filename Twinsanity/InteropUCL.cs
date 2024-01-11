using System.Collections.Generic;
namespace Twinsanity
{
    public static class InteropUCL
    {
        public static byte[] DecompressNRV2B(byte[] data)
        {
            List<byte> buffer = new List<byte>();
            BitReader reader = new BitReader(data);
            uint lastmOff = 1;
            while (true)
            {
                uint mOff = 1;
                uint mLen = 0;
                if (reader.EndOfData()) break;
                while (reader.ReadBit() != 0)
                {
                    if (reader.EndOfData()) break;
                    buffer.Add(reader.ReadByte());
                    if (reader.EndOfData()) break;
                }
                if (reader.EndOfData()) break;
                do
                {
                    if (reader.EndOfData()) break;
                    mOff = mOff * 2 + reader.ReadBit();
                    if (reader.EndOfData()) break;
                } while (reader.ReadBit() == 0);
                if (reader.EndOfData()) break;
                if (mOff == 2)
                {
                    mOff = lastmOff;
                }
                else
                {
                    mOff = (mOff - 3) * 256 + reader.ReadByte();
                    if (mOff == 0xFFFFFFFF) break;
                    if (reader.EndOfData()) break;
                    lastmOff = ++mOff;
                }
                mLen = reader.ReadBit();
                if (reader.EndOfData()) break;
                mLen = mLen * 2 + reader.ReadBit();
                if (reader.EndOfData()) break;
                if (mLen == 0)
                {
                    mLen++;
                    do
                    {
                        if (reader.EndOfData()) break;
                        mLen = mLen * 2 + reader.ReadBit();
                        if (reader.EndOfData()) break;
                    } while (reader.ReadBit() == 0);
                    if (reader.EndOfData()) break;
                    mLen += 2;
                }
                if (mOff > 0xD00) mLen += 1;
                int mPos = (int)(buffer.Count - mOff);
                buffer.Add(buffer[mPos++]);
                do buffer.Add(buffer[mPos++]); while (--mLen > 0);
            }

            byte[] outData = buffer.ToArray();
            return outData;
        }
    }

    public class BitReader
    {
        byte[] data;
        int bitPos;
        int curByte;
        public int dataPos;

        public BitReader(byte[] d)
        {
            data = d;
        }

        public byte ReadByte()
        {
            return (byte)(data[dataPos++] & 0xFF);
        }

        public byte ReadBit()
        {
            if (bitPos == 0)
            {
                curByte = data[dataPos++] & 0xFF;
                bitPos = 8;
            }
            return (byte)(((uint)curByte >> --bitPos) & 1);
        }

        public bool EndOfData()
        {
            return dataPos >= data.Length;
        }
    }
}
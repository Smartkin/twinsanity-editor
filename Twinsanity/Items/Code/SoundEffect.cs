using System;
using System.IO;

namespace Twinsanity
{
    public class SoundEffect : TwinsItem
    {
        private static readonly double k = ((22050.0f / 1881.0f) + (44100.0f / 3763.0f) + (48000.0f / 4096.0f)) / 3.0f;

        public uint Head { get; set; }
        public byte UnkFlag { get; set; }
        public byte FreqFac { get; set; }
        public ushort Param1 { get; set; }
        public ushort Param2 { get; set; }
        public ushort Param3 { get; set; }
        public ushort Param4 { get; set; }
        public uint SoundSize { get; set; }
        public uint SoundOffset { get; set; }

        public ushort Freq { get => GetFreq(FreqFac); }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Head);
            writer.Write(UnkFlag);
            writer.Write(FreqFac);
            writer.Write(Param1);
            writer.Write(Param2);
            writer.Write(Param3);
            writer.Write(Param4);
            writer.Write(SoundSize);
            writer.Write(SoundOffset);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Head = reader.ReadUInt32();
            UnkFlag = reader.ReadByte();
            FreqFac = reader.ReadByte();
            Param1 = reader.ReadUInt16();
            Param2 = reader.ReadUInt16();
            Param3 = reader.ReadUInt16();
            Param4 = reader.ReadUInt16();
            SoundSize = reader.ReadUInt32();
            SoundOffset = reader.ReadUInt32();
        }

        protected override int GetSize()
        {
            return 22;
        }

        public static ushort GetFreq(ushort freq)
        {
            switch (freq)
            {
                case 0x2:
                    return 8000;
                case 0x3:
                    return 10000;
                case 0x4:
                    return 11025;
                case 0x5:
                    return 16000;
                case 0x6:
                    return 18000;
                case 0x7:
                    return 22050;
                //case 0x8:
                //    return 24000;
                case 0xA:
                    return 32000;
                case 0xE:
                    return 44100;
                case 0x10:
                    return 48000;
                default:
                    //return (ushort)Math.Round(freq * 0x100 * k);
                    throw new ArgumentException($"Unhandled sfx frequency. Value was: {freq}", "freq");
            }
        }
    }
}

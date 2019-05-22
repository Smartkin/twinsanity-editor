using System;
using System.IO;

namespace Twinsanity
{
    public class SoundEffect : TwinsItem
    {
        private static readonly double k = ((22050.0f / 1881.0f) + (44100.0f / 3763.0f) + (48000.0f / 4096.0f)) / 3.0f;

        public uint Head { get; set; }
        public ushort Frequency { get; set; }
        public ushort Param1 { get; set; }
        public ushort Param2 { get; set; }
        public ushort Param3 { get; set; }
        public ushort Param4 { get; set; }
        public uint SoundSize { get; set; }
        public uint SoundOffset { get; set; }

        public ushort Freq { get => GetFreq(); }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Head);
            writer.Write(Frequency);
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
            Frequency = reader.ReadUInt16();
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

        private ushort GetFreq()
        {
            switch (Frequency)
            {
                case 682:
                    return 8000;
                case 1024:
                    return 11025;
                case 1365:
                    return 16000;
                case 1536:
                    return 18000;
                case 1706:
                    return 20000;
                case 1881:
                    return 22050;
                case 3763:
                    return 44100;
                case 4096:
                    return 48000;
                default:
                    return (ushort)System.Math.Round(Frequency * k);
            }
        }

        public static ushort GetFreq(ushort freq)
        {
            switch (freq)
            {
                case 682:
                    return 8000;
                case 1024:
                    return 11025;
                case 1365:
                    return 16000;
                case 1536:
                    return 18000;
                case 1706:
                    return 20000;
                case 1881:
                    return 22050;
                case 3763:
                    return 44100;
                case 4096:
                    return 48000;
                default:
                    return (ushort)Math.Round(freq * k);
            }
        }
    }
}

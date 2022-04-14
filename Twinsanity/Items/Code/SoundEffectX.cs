using System;
using System.IO;

namespace Twinsanity
{
    public class SoundEffectX : TwinsItem
    {
        public uint Head { get; set; }
        public byte[] SoundData { get; set; }
        public ushort Freq { get; set; }
        public byte[] Header { get; set; }
        public uint UnkInt { get; set; }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Head);
            writer.Write((uint)Freq);
            writer.Write(Header);
            writer.Write(SoundData.Length);
            writer.Write(UnkInt);
            writer.Write(SoundData);
            writer.Write(SoundData.Length);
            writer.Write((uint)0);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Head = reader.ReadUInt32();
            Freq = (ushort)reader.ReadUInt32();
            Header = reader.ReadBytes(0x38);
            int SoundSize = reader.ReadInt32();
            UnkInt = reader.ReadUInt32();
            SoundSize -= 4;
            SoundData = reader.ReadBytes(SoundSize);
            reader.ReadBytes(8); //SoundSize again and zero
        }

        protected override int GetSize()
        {
            return 4 + 4 + 0x38 + 4 + 4 + SoundData.Length + 8;
        }

    }
}

using System;
using System.IO;

namespace Twinsanity
{
    public class SoundEffectMB : SoundEffect
    {

        public uint FreqReal { get; set; }

        public override ushort Freq
        {
            get => (ushort)FreqReal;
            set
            {
                FreqReal = value;
            }
        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Head);
            writer.Write(Freq);
            writer.Write(SoundSize);
            writer.Write(SoundOffset);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Head = reader.ReadUInt32();
            FreqReal = reader.ReadUInt32();
            SoundSize = reader.ReadUInt32();
            SoundOffset = reader.ReadUInt32();
        }

        protected override int GetSize()
        {
            return 16;
        }
    }
}

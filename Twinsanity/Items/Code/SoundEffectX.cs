using System;
using System.IO;

namespace Twinsanity
{
    public class SoundEffectX : TwinsItem
    {
        public byte[] SoundData { get; set; }
        public ushort Freq { get; set; }
        public int UnkInt { get; set; } // sometimes SoundSize again, sometimes 0, sometimes negative

        // Confirmed static (possibly includes mono/stereo flag, but none of the sounds are stereo anyway)
        static byte[] HeaderStatic1 = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00 };
        static byte[] HeaderStatic2 = new byte[] { 0x02, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public override void Save(BinaryWriter writer)
        {
            writer.Write((uint)3);
            writer.Write((uint)Freq);
            writer.Write(HeaderStatic1);
            writer.Write((uint)Freq);
            writer.Write((uint)Freq * 2);
            writer.Write(HeaderStatic2);
            writer.Write(SoundData.Length + 4);
            writer.Write(UnkInt);
            writer.Write(SoundData);
            writer.Write(SoundData.Length + 4);
            writer.Write((uint)0);
        }

        public override void Load(BinaryReader reader, int size)
        {
            reader.ReadUInt32(); // Confirmed always 3
            Freq = (ushort)reader.ReadUInt32();
            reader.ReadBytes(HeaderStatic1.Length + HeaderStatic2.Length + 8); // HeaderStatic1, Freq again, Freq multiplied by 2, HeaderStatic2
            int SoundSize = reader.ReadInt32() - 4;
            UnkInt = reader.ReadInt32();
            SoundData = reader.ReadBytes(SoundSize);
            reader.ReadBytes(8); //SoundSize again and zero
        }

        protected override int GetSize()
        {
            return 0x50 + SoundData.Length;
        }

    }
}

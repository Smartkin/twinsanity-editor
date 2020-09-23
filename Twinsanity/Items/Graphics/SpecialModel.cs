using System.Collections.Generic;
using System;
using System.IO;

namespace Twinsanity
{
    public sealed class SpecialModel : TwinsItem
    {

        public uint Header;
        public uint LodAmount;
        public int UnkInt1;
        public int UnkInt2;
        public byte[] UnkData;
        public uint[] LODModelIDs; // 4

        public long DataSize;

        public SpecialModel()
        {

        }

        protected override int GetSize()
        {
            return 21 + LODModelIDs.Length * 4;
        }

        /// <summary>
        /// Write converted binary data to file.
        /// </summary>
        public override void Save(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write((byte)LodAmount);
            writer.Write(UnkInt1);
            writer.Write(UnkInt2);
            writer.Write(UnkData);
            for (int i = 0; i < LodAmount; ++i)
            {
                writer.Write(LODModelIDs[i]);
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            Header = reader.ReadUInt32();
            LodAmount = reader.ReadByte();
            UnkInt1 = reader.ReadInt32();
            UnkInt2 = reader.ReadInt32();
            UnkData = reader.ReadBytes(0xC);
            LODModelIDs = new uint[LodAmount];
            for (int i = 0; i < LodAmount; ++i)
            {
                LODModelIDs[i] = reader.ReadUInt32();
            }
        }

    }
}

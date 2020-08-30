using System.Collections.Generic;
using System;
using System.IO;

namespace Twinsanity
{
    public sealed class SpecialModel : TwinsItem
    {

        public uint Header;
        public uint K_Count;
        public uint[] K_Val;
        public uint[] LODModelIDs; // 4

        public long DataSize;

        public SpecialModel()
        {

        }

        protected override int GetSize()
        {
            int count = 4 + 4 + 1 + 4 + 4 + 4 + 4;
            count += (int)(K_Count * 4);
            return count;
        }

        /// <summary>
        /// Write converted binary data to file.
        /// </summary>
        public override void Save(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write(K_Count);
            writer.Write((byte)0x00);
            for (int i = 0; i < K_Count; i++)
            {
                writer.Write(K_Val[i]);
            }
            writer.Write(LODModelIDs[0]);
            writer.Write(LODModelIDs[1]);
            writer.Write(LODModelIDs[2]);
            writer.Write(LODModelIDs[3]);
        }

        public override void Load(BinaryReader reader, int size)
        {
            long start_pos = reader.BaseStream.Position;

            Header = reader.ReadUInt32();
            K_Count = reader.ReadUInt32();
            reader.ReadByte();
            K_Val = new uint[K_Count];
            for (int i = 0; i < K_Count; i++)
            {
                K_Val[i] = reader.ReadUInt32();
            }
            LODModelIDs = new uint[4];
            LODModelIDs[0] = reader.ReadUInt32();
            LODModelIDs[1] = reader.ReadUInt32();
            LODModelIDs[2] = reader.ReadUInt32();
            LODModelIDs[3] = reader.ReadUInt32();
        }

    }
}

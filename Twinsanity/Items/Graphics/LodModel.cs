using System.Collections.Generic;
using System;
using System.IO;

namespace Twinsanity
{
    public class LodModel : TwinsItem
    {

        public uint Header = 4098;
        public uint ModelsAmount = 1;
        public uint Zero;
        public uint[] LODDistance = new uint[4] { 1641631945, 1641631945, 1641631945, 1641631945 }; // 4
        public uint[] LODModelIDs = new uint[1] { 0, }; // 4

        public LodModel()
        {

        }

        protected override int GetSize()
        {
            return 25 + LODModelIDs.Length * 4;
        }

        /// <summary>
        /// Write converted binary data to file.
        /// </summary>
        public override void Save(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write((byte)ModelsAmount);
            writer.Write(Zero);
            for (int i = 0; i < LODDistance.Length; ++i)
            {
                writer.Write(LODDistance[i]);
            }
            for (int i = 0; i < ModelsAmount; ++i)
            {
                writer.Write(LODModelIDs[i]);
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            Header = reader.ReadUInt32();
            ModelsAmount = reader.ReadByte();
            Zero = reader.ReadUInt32();
            LODDistance = new uint[4];
            for (int i = 0; i < LODDistance.Length; ++i)
            {
                LODDistance[i] = reader.ReadUInt32();
            }
            LODModelIDs = new uint[ModelsAmount];
            for (int i = 0; i < ModelsAmount; ++i)
            {
                LODModelIDs[i] = reader.ReadUInt32();
            }
        }

    }
}

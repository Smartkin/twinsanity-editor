using System.Collections.Generic;
using System;
using System.IO;

namespace Twinsanity
{
    public class LodModelMB : LodModel
    {

        /// <summary>
        /// Write converted binary data to file.
        /// </summary>
        public override void Save(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write(ModelsAmount);
            for (int i = 0; i < LODDistance.Length; ++i)
            {
                writer.Write(LODDistance[i]);
            }
            for (int i = 0; i < ModelsAmount; ++i)
            {
                writer.Write(LODModelIDs[i]);
            }
            writer.Write((byte)Zero);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Header = reader.ReadUInt32();
            ModelsAmount = reader.ReadUInt32();
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
            Zero = reader.ReadByte();
        }

    }
}

using System.Collections.Generic;
using System.IO;
using System;

namespace Twinsanity
{
    public class ColDataMB : ColData
    {
        protected override int GetSize()
        {
            return isEmpty ? 0 : (36 + Triggers.Count * 32 + Groups.Count * 8 + Tris.Count * 8 + Vertices.Count * 16);
        }

        /// <summary>
        /// Write converted binary data to file.
        /// </summary>
        public override void Save(BinaryWriter writer)
        {
            if (isEmpty) return;
            throw new NotImplementedException();
        }

        public override void Load(BinaryReader areader, int size)
        {
            if (size < 36)
            {
                isEmpty = true;
                return;
            }
            someNumber = areader.ReadUInt32();
            uint triggerCount = areader.ReadUInt32();
            uint groupCount = areader.ReadUInt32();
            uint triCount = areader.ReadUInt32();
            uint vertexCount = areader.ReadUInt32();
            Triggers.Clear();
            Groups.Clear();
            Tris.Clear();
            Vertices.Clear();
            uint triggerSize = areader.ReadUInt32();
            uint groupSize = areader.ReadUInt32();
            uint triSize = areader.ReadUInt32();
            uint vertexSize = areader.ReadUInt32();
            try
            {
                areader.ReadBytes(4); // PACK
                byte[] outData = InteropUCL.DecompressNRV2B(areader.ReadBytes((int)triggerSize - 4));
                using (MemoryStream subMem = new MemoryStream(outData))
                using (BinaryReader reader = new BinaryReader(subMem))
                {
                    for (int i = 0; i < triggerCount; i++)
                    {
                        Trigger trg = new Trigger
                        {
                            X1 = reader.ReadSingle(),
                            Y1 = reader.ReadSingle(),
                            Z1 = reader.ReadSingle(),
                            Flag1 = reader.ReadInt32(),
                            X2 = reader.ReadSingle(),
                            Y2 = reader.ReadSingle(),
                            Z2 = reader.ReadSingle(),
                            Flag2 = reader.ReadInt32()
                        };
                        Triggers.Add(trg);
                    }
                }
            }
            catch
            {
                Console.WriteLine($"Failed to unpack triggers.");
            }
            try
            {
                areader.ReadBytes(4); // PACK
                byte[] outData = InteropUCL.DecompressNRV2B(areader.ReadBytes((int)groupSize - 4));
                using (MemoryStream subMem = new MemoryStream(outData))
                using (BinaryReader reader = new BinaryReader(subMem))
                {
                    for (int i = 0; i < groupCount; i++)
                    {
                        GroupInfo grp = new GroupInfo
                        {
                            Size = reader.ReadUInt32(),
                            Offset = reader.ReadUInt32()
                        };
                        Groups.Add(grp);
                    }
                }
            }
            catch
            {
                Console.WriteLine($"Failed to unpack groups.");
            }
            try
            {
                areader.ReadBytes(4); // PACK
                byte[] outData = InteropUCL.DecompressNRV2B(areader.ReadBytes((int)triSize - 4));
                using (MemoryStream subMem = new MemoryStream(outData))
                using (BinaryReader reader = new BinaryReader(subMem))
                {
                    for (int i = 0; i < triCount; i++)
                    {
                        ColTri tri = new ColTri();
                        ulong legacy = reader.ReadUInt64();
                        tri.Vert1 = (int)(legacy & mask);
                        tri.Vert2 = (int)((legacy >> 18 * 1) & mask);
                        tri.Vert3 = (int)((legacy >> 18 * 2) & mask);
                        tri.Surface = (int)(legacy >> (18 * 3));
                        Tris.Add(tri);
                    }
                }
            }
            catch
            {
                Console.WriteLine($"Failed to unpack tris.");
            }
            try
            {
                areader.ReadBytes(4); // PACK
                byte[] outData = InteropUCL.DecompressNRV2B(areader.ReadBytes((int)vertexSize - 4));
                using (MemoryStream subMem = new MemoryStream(outData))
                using (BinaryReader reader = new BinaryReader(subMem))
                {
                    for (int i = 0; i < vertexCount; i++)
                    {
                        Pos vtx = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                        Vertices.Add(vtx);
                    }
                }
            }
            catch
            {
                Console.WriteLine($"Failed to unpack verts.");
            }
        }

    }
}

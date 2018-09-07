using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public sealed class ColData : TwinsItem
    {
        private uint someNumber;
        private List<Trigger> triggers;
        private List<GroupInfo> groups;
        private List<ColTri> tris;
        private List<Pos> vertices;
        private readonly uint mask = 0x3FFFF;

        public ColData()
        {
            triggers = new List<Trigger>();
            groups = new List<GroupInfo>();
            tris = new List<ColTri>();
            vertices = new List<Pos>();
        }

        protected override int GetSize()
        {
            return (20 + triggers.Count * 32 + groups.Count * 8 + tris.Count * 8 + vertices.Count * 16);
        }

        /// <summary>
        /// Update the object's memory stream with new data
        /// </summary>
        public override byte[] Save()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            if (someNumber > 0)
            {
                writer.Write(someNumber);
                writer.Write(triggers.Count);
                writer.Write(groups.Count);
                writer.Write(tris.Count);
                writer.Write(vertices.Count);
                for (int i = 0; i < triggers.Count; i++)
                {
                    writer.Write(triggers[i].X1);
                    writer.Write(triggers[i].Y1);
                    writer.Write(triggers[i].Z1);
                    writer.Write(triggers[i].Flag1);
                    writer.Write(triggers[i].X2);
                    writer.Write(triggers[i].Y2);
                    writer.Write(triggers[i].Z2);
                    writer.Write(triggers[i].Flag2);
                }
                for (int i = 0; i < groups.Count; i++)
                {
                    writer.Write(groups[i].Size);
                    writer.Write(groups[i].Offset);
                }
                for (int i = 0; i < tris.Count; i++)
                {
                    long tmp = ((uint)tris[i].Vert1 & mask) |
                        (((uint)tris[i].Vert2 & mask) << 18) |
                        (((uint)tris[i].Vert3 & mask) << 18*2) |
                        (((uint)tris[i].Surface & 0x3FF) << 18*3);
                    writer.Write(tmp);
                }
                for (int i = 0; i < vertices.Count; i++)
                {
                    writer.Write(vertices[i].X);
                    writer.Write(vertices[i].Y);
                    writer.Write(vertices[i].Z);
                    writer.Write(vertices[i].W);
                }
            }
            return stream.ToArray();
        }

        /////////PARENTS FUNCTION//////////
        public override void Load(BinaryReader reader)
        {
            someNumber = reader.ReadUInt32();
            uint triggerCount = reader.ReadUInt32();
            uint groupCount = reader.ReadUInt32();
            uint triCount = reader.ReadUInt32();
            uint vertexCount = reader.ReadUInt32();
            triggers.Clear();
            groups.Clear();
            tris.Clear();
            vertices.Clear();
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
                triggers.Add(trg);
            }
            for (int i = 0; i < groupCount; i++)
            {
                GroupInfo grp = new GroupInfo
                {
                    Size = reader.ReadUInt32(),
                    Offset = reader.ReadUInt32()
                };
                groups.Add(grp);
            }
            for (int i = 0; i < triCount; i++)
            {
                ColTri tri = new ColTri();
                ulong legacy = reader.ReadUInt64();
                tri.Vert1 = (int)(legacy & mask);
                tri.Vert2 = (int)((legacy >> 18 * 1) & mask);
                tri.Vert3 = (int)((legacy >> 18 * 2) & mask);
                tri.Surface = (int)(legacy >> (18 * 3));
                tris.Add(tri);
            }
            for (int i = 0; i < vertexCount; i++)
            {
                Pos vtx = new Pos
                {
                    X = reader.ReadSingle(),
                    Y = reader.ReadSingle(),
                    Z = reader.ReadSingle(),
                    W = reader.ReadSingle()
                };
                vertices.Add(vtx);
            }
        }

        #region STRUCTURES
        public struct Trigger
        {
            public float X1;
            public float Y1;
            public float Z1;
            public int Flag1;
            public float X2;
            public float Y2;
            public float Z2;
            public int Flag2;
        }
        public struct GroupInfo
        {
            public uint Size;
            public uint Offset;
        }
        public struct ColTri
        {
            public int Vert1;
            public int Vert2;
            public int Vert3;
            public int Surface;
        }
        #endregion

        public List<Trigger> Triggers
        {
            get { return triggers; }
            set { triggers = value; }
        }
        public List<GroupInfo> Groups
        {
            get { return groups; }
            set { groups = value; }
        }
        public List<ColTri> Tris
        {
            get { return tris; }
            set { tris = value; }
        }
        public List<Pos> Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }
    }
}

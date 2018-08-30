using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public class GeoData : BaseItem
    {
        private new readonly string NodeName = "Collision Tree Data";
        private uint someNumber;
        //private uint triggerCount;
        //private uint groupCount;
        //private uint triCount;
        //private uint vertexCount;
        private List<Trigger> triggers;
        private List<GroupInfo> groups;
        private List<ColTri> tris;
        private List<Pos> vertices;
        private readonly uint mask = 0x3FFFF;

        public GeoData()
        {
            this.triggers = new List<Trigger>();
            this.groups = new List<GroupInfo>();
            this.tris = new List<ColTri>();
            this.vertices = new List<Pos>();
        }

        /// <summary>
        /// Update the object's memory stream with new data
        /// </summary>
        public override void UpdateStream()
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
                    long tmp = (tris[i].Vert1 & mask) |
                        ((tris[i].Vert2 & mask) << 18) |
                        ((tris[i].Vert3 & mask) << 18*2) |
                        ((tris[i].Surface & 0x3FF) << 18*3);
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
            else
                writer.Write(ByteStream.ToArray());
            ByteStream = stream;
            Size = (uint)ByteStream.Length;
        }

        /////////PARENTS FUNCTION//////////
        protected override void DataUpdate()
        {
            BinaryReader BSReader = new BinaryReader(ByteStream);
            ByteStream.Position = 0;
            if (ByteStream.Length > 0)
            {
                someNumber = BSReader.ReadUInt32();
                uint triggerCount = BSReader.ReadUInt32();
                uint groupCount = BSReader.ReadUInt32();
                uint triCount = BSReader.ReadUInt32();
                uint vertexCount = BSReader.ReadUInt32();
                triggers.Clear();
                groups.Clear();
                tris.Clear();
                vertices.Clear();
                for (int i = 0; i < triggerCount; i++)
                {
                    Trigger trg = new Trigger
                    {
                        X1 = BSReader.ReadSingle(),
                        Y1 = BSReader.ReadSingle(),
                        Z1 = BSReader.ReadSingle(),
                        Flag1 = BSReader.ReadInt32(),
                        X2 = BSReader.ReadSingle(),
                        Y2 = BSReader.ReadSingle(),
                        Z2 = BSReader.ReadSingle(),
                        Flag2 = BSReader.ReadInt32()
                    };
                    triggers.Add(trg);
                }
                for (int i = 0; i < groupCount; i++)
                {
                    GroupInfo grp = new GroupInfo
                    {
                        Size = BSReader.ReadUInt32(),
                        Offset = BSReader.ReadUInt32()
                    };
                    groups.Add(grp);
                }
                for (int i = 0; i < triCount; i++)
                {
                    ColTri tri = new ColTri();
                    ulong legacy = BSReader.ReadUInt64();
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
                        X = BSReader.ReadSingle(),
                        Y = BSReader.ReadSingle(),
                        Z = BSReader.ReadSingle(),
                        W = BSReader.ReadSingle()
                    };
                    vertices.Add(vtx);
                }
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

using System;

namespace Twinsanity
{
    public class GeoData : BaseItem
    {
        public new string NodeName = "Geometrical Data";
        public uint SomeNumber;
        public int TriggerSize;
        public int GroupSize;
        public int IndexSize;
        public int VertexSize;
        public VertexMaps[] Triggers;
        public OffsetsMapping[] Groups;
        public Index[] Indexes;
        public RM2.Coordinate4[] Vertex;

        /// <summary>
        /// Update the object's memory stream with new data
        /// </summary>
        public override void UpdateStream()
        {
            System.IO.MemoryStream NewStream = new System.IO.MemoryStream();
            System.IO.BinaryWriter NSWriter = new System.IO.BinaryWriter(NewStream);
            if (SomeNumber > 0)
            {
                NSWriter.Write(SomeNumber);
                NSWriter.Write(TriggerSize);
                NSWriter.Write(GroupSize);
                NSWriter.Write(IndexSize);
                NSWriter.Write(VertexSize);
                for (int i = 0; i <= TriggerSize - 1; i++)
                {
                    NSWriter.Write(Triggers[i].X1);
                    NSWriter.Write(Triggers[i].Y1);
                    NSWriter.Write(Triggers[i].Z1);
                    NSWriter.Write(Triggers[i].Flag1);
                    NSWriter.Write(Triggers[i].X2);
                    NSWriter.Write(Triggers[i].Y2);
                    NSWriter.Write(Triggers[i].Z2);
                    NSWriter.Write(Triggers[i].Flag2);
                }
                for (int i = 0; i <= GroupSize - 1; i++)
                {
                    NSWriter.Write(Groups[i].Size);
                    NSWriter.Write(Groups[i].Offset);
                }
                for (int i = 0; i <= IndexSize - 1; i++)
                {
                    ulong tmp1, tmp2, tmp3, tmp4;
                    tmp1 = Indexes[i].Vert1;
                    tmp2 = Indexes[i].Vert2;
                    tmp2 = tmp2 << 18;
                    tmp3 = Indexes[i].Vert3;
                    tmp3 = tmp3 << 36;
                    tmp4 = Indexes[i].Surface;
                    tmp4 = tmp4 << 54;
                    Indexes[i].legacy = tmp1 + tmp2 + tmp3 + tmp4;
                    NSWriter.Write(Indexes[i].legacy);
                }
                for (int i = 0; i <= VertexSize - 1; i++)
                {
                    NSWriter.Write(Vertex[i].X);
                    NSWriter.Write(Vertex[i].Y);
                    NSWriter.Write(Vertex[i].Z);
                    NSWriter.Write(Vertex[i].W);
                }
            }
            else
                NSWriter.Write(ByteStream.ToArray());
            ByteStream = NewStream;
            Size = (uint)ByteStream.Length;
        }

        /////////PARENTS FUNCTION//////////
        protected override void DataUpdate()
        {
            System.IO.BinaryReader BSReader = new System.IO.BinaryReader(ByteStream);
            ByteStream.Position = 0;
            if (ByteStream.Length > 0)
            {
                SomeNumber = BSReader.ReadUInt32();
                TriggerSize = (int)BSReader.ReadUInt32();
                GroupSize = (int)BSReader.ReadUInt32();
                IndexSize = (int)BSReader.ReadUInt32();
                VertexSize = (int)BSReader.ReadUInt32();
                Array.Resize(ref Triggers, TriggerSize);
                Array.Resize(ref Groups, GroupSize);
                Array.Resize(ref Indexes, IndexSize);
                Array.Resize(ref Vertex, VertexSize);
                for (int i = 0; i <= TriggerSize - 1; i++)
                {
                    Triggers[i].X1 = BSReader.ReadSingle();
                    Triggers[i].Y1 = BSReader.ReadSingle();
                    Triggers[i].Z1 = BSReader.ReadSingle();
                    Triggers[i].Flag1 = BSReader.ReadInt32();
                    Triggers[i].X2 = BSReader.ReadSingle();
                    Triggers[i].Y2 = BSReader.ReadSingle();
                    Triggers[i].Z2 = BSReader.ReadSingle();
                    Triggers[i].Flag2 = BSReader.ReadInt32();
                }
                for (int i = 0; i <= GroupSize - 1; i++)
                {
                    Groups[i].Size = BSReader.ReadUInt32();
                    Groups[i].Offset = BSReader.ReadUInt32();
                }
                for (int i = 0; i <= IndexSize - 1; i++)
                {
                    Indexes[i].legacy = BSReader.ReadUInt64();
                    ulong mask = 262143;
                    ulong tmp = Indexes[i].legacy & mask;
                    Indexes[i].Vert1 = (uint)tmp;
                    mask = mask << 18;
                    tmp = Indexes[i].legacy & mask;
                    tmp = tmp >> 18;
                    Indexes[i].Vert2 = (uint)tmp;
                    mask = mask << 18;
                    tmp = Indexes[i].legacy & mask;
                    tmp = tmp >> 36;
                    Indexes[i].Vert3 = (uint)tmp;
                    mask = 1023;
                    mask = mask << 54;
                    tmp = Indexes[i].legacy & mask;
                    tmp = tmp >> 54;
                    Indexes[i].Surface = (uint)tmp;
                }
                for (int i = 0; i <= VertexSize - 1; i++)
                {
                    Vertex[i].X = BSReader.ReadSingle();
                    Vertex[i].Y = BSReader.ReadSingle();
                    Vertex[i].Z = BSReader.ReadSingle();
                    Vertex[i].W = BSReader.ReadSingle();
                }
            }
        }

        #region STRUCTURES
        public struct VertexMaps
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
        public struct OffsetsMapping
        {
            public uint Size;
            public uint Offset;
        }
        public struct Index
        {
            public ulong legacy;
            public uint Vert1;
            public uint Vert2;
            public uint Vert3;
            public uint Surface;
        }
        #endregion
    }
}

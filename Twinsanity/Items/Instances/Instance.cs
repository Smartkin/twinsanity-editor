using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public class Instance : TwinsItem
    {
        private Pos pos;
        private List<ushort> s1 = new List<ushort>(), s2 = new List<ushort>(), s3 = new List<ushort>();

        public Pos Pos { get => pos; set => pos = value; }
        public ushort RotX { get; set; }
        public ushort RotY { get; set; }
        public ushort RotZ { get; set; }
        public ushort COMRotX { get; set; }
        public ushort COMRotY { get; set; }
        public ushort COMRotZ { get; set; }
        public List<ushort> S1 { get => s1; set => s1 = value; }
        public List<ushort> S2 { get => s2; set => s2 = value; }
        public List<ushort> S3 { get => s3; set => s3 = value; }
        public int SomeNum1 { get; set; }
        public int SomeNum2 { get; set; }
        public int SomeNum3 { get; set; }
        public ushort ObjectID { get; set; }
        public uint AfterOID { get; set; }
        public uint PHeader { get; set; }
        public uint UnkI32 { get; set; }
        public uint[] UnkI321 { get; set; }
        public float[] UnkI322 { get; set; }
        public uint[] UnkI323 { get; set; }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Pos.X);
            writer.Write(Pos.Y);
            writer.Write(Pos.Z);
            writer.Write(Pos.W);
            writer.Write(RotX);
            writer.Write(COMRotX);
            writer.Write(RotY);
            writer.Write(COMRotY);
            writer.Write(RotZ);
            writer.Write(COMRotZ);
            writer.Write(S1.Count);
            writer.Write(S1.Count);
            writer.Write(SomeNum1);
            for (int i = 0; i < S1.Count; ++i)
                writer.Write(S1[i]);
            writer.Write(S2.Count);
            writer.Write(S2.Count);
            writer.Write(SomeNum2);
            for (int i = 0; i < S2.Count; ++i)
                writer.Write(S2[i]);
            writer.Write(S3.Count);
            writer.Write(S3.Count);
            writer.Write(SomeNum3);
            for (int i = 0; i < S3.Count; ++i)
                writer.Write(S3[i]);
            writer.Write(ObjectID);
            writer.Write(AfterOID);
            writer.Write(PHeader);
            PHeader = (uint)((byte)UnkI321.Length
                | (UnkI322.Length << 8)
                | (UnkI323.Length << 16));
            writer.Write(UnkI32);
            writer.Write(UnkI321.Length);
            for (int i = 0; i < UnkI321.Length; ++i)
                writer.Write(UnkI321[i]);
            writer.Write(UnkI322.Length);
            for (int i = 0; i < UnkI322.Length; ++i)
                writer.Write(UnkI322[i]);
            writer.Write(UnkI323.Length);
            for (int i = 0; i < UnkI323.Length; ++i)
                writer.Write(UnkI323[i]);
        }

        public override void Load(BinaryReader reader, int size)
        {
            pos = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            RotX = reader.ReadUInt16();
            COMRotX = reader.ReadUInt16();
            RotY = reader.ReadUInt16();
            COMRotY = reader.ReadUInt16();
            RotZ = reader.ReadUInt16();
            COMRotZ = reader.ReadUInt16();

            var n = reader.ReadInt32();
            n = reader.ReadInt32();
            SomeNum1 = reader.ReadInt32();
            S1.Clear();
            for (int i = 0; i < n; ++i)
                S1.Add(reader.ReadUInt16());
            n = reader.ReadInt32();
            n = reader.ReadInt32();
            SomeNum2 = reader.ReadInt32();
            S2.Clear();
            for (int i = 0; i < n; ++i)
                S2.Add(reader.ReadUInt16());
            n = reader.ReadInt32();
            n = reader.ReadInt32();
            SomeNum3 = reader.ReadInt32();
            S3.Clear();
            for (int i = 0; i < n; ++i)
                S3.Add(reader.ReadUInt16());
            ObjectID = reader.ReadUInt16();
            AfterOID = reader.ReadUInt32();
            PHeader = reader.ReadUInt32();
            UnkI32 = reader.ReadUInt32();
            n = reader.ReadInt32();
            UnkI321 = new uint[n];
            for (int i = 0; i < n; ++i)
                UnkI321[i] = reader.ReadUInt32();
            n = reader.ReadInt32();
            UnkI322 = new float[n];
            for (int i = 0; i < n; ++i)
                UnkI322[i] = reader.ReadSingle();
            n = reader.ReadInt32();
            UnkI323 = new uint[n];
            for (int i = 0; i < n; ++i)
                UnkI323[i] = reader.ReadUInt32();
        }

        protected override int GetSize()
        {
            return 90 + (S1.Count + S2.Count + S3.Count) * 2 + (UnkI321.Length + UnkI322.Length + UnkI323.Length) * 4;
        }
    }
}

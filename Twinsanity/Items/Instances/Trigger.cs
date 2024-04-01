using System.IO;
using System.Collections.Generic;

namespace Twinsanity
{
    public class Trigger : TwinsItem
    {
        public uint Header { get; set; }

        public bool Arg1_Used
        {
            get
            {
                return (Header >> 0xB & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0xB;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool Arg2_Used
        {
            get
            {
                return (Header >> 0x8 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x8;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool Arg3_Used
        {
            get
            {
                return (Header >> 0x9 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x9;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool Arg4_Used
        {
            get
            {
                return (Header >> 0xA & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0xA;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }

        public bool[] Mask
        {
            get
            {
                bool[] m = new bool[9] { false, false, false, false, false, false, false, false, false };

                for (int i = 0; i < m.Length; i++)
                {
                    if ((Enabled >> i & 0x1) != 0)
                    {
                        m[i] = true;
                    }
                }

                return m;
            }
            set
            {
                Enabled = 0;

                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i])
                    {
                        Enabled |= (uint)(1 << i);
                    }
                }
            }
        }

        public uint Enabled { get; set; }
        public float SomeFloat { get; set; }
        public Pos[] Coords { get; set; } = new Pos[3]; // rot/pos/size
        public uint SectionHead { get; set; }
        public List<ushort> Instances { get; set; }

        public ushort Arg1 { get; set; }
        public ushort Arg2 { get; set; }
        public ushort Arg3 { get; set; }
        public ushort Arg4 { get; set; }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write(Enabled);
            writer.Write(SomeFloat);
            for (int i = 0; i < 3; ++i)
            {
                writer.Write(Coords[i].X);
                writer.Write(Coords[i].Y);
                writer.Write(Coords[i].Z);
                writer.Write(Coords[i].W);
            }
            writer.Write(Instances.Count);
            writer.Write(Instances.Count);
            writer.Write(SectionHead);
            for (int i = 0; i < Instances.Count; ++i)
                writer.Write(Instances[i]);

            writer.Write(Arg1);
            writer.Write(Arg2);
            writer.Write(Arg3);
            writer.Write(Arg4);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Header = reader.ReadUInt32();
            Enabled = reader.ReadUInt32();
            SomeFloat = reader.ReadSingle();
            for (int i = 0; i < 3; ++i)
            {
                Coords[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            var n = reader.ReadInt32();
            n = reader.ReadInt32();
            SectionHead = reader.ReadUInt32();
            Instances = new List<ushort>(n);
            for (int i = 0; i < n; ++i)
                Instances.Add(reader.ReadUInt16());

            Arg1 = reader.ReadUInt16();
            Arg2 = reader.ReadUInt16();
            Arg3 = reader.ReadUInt16();
            Arg4 = reader.ReadUInt16();
        }

        protected override int GetSize()
        {
            return 80 + Instances.Count * 2;
        }
    }
}

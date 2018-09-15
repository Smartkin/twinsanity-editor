using System.IO;

namespace Twinsanity
{
    public class Material : TwinsItem
    {
        public string Name { get; set; }
        public int Header { get; set; }
        public int Unknown { get; set; }
        public int Type { get; set; }
        public uint Tex { get; set; }
        public uint Last { get; set; }
        public byte[] UnkArray1 { get; set; }
        public byte[] UnkArray2 { get; set; }
        public float[] ValuesF { get; set; } = new float[4];
        public int[] ValuesI { get; set; } = new int[4];


        public override void Save(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write(Unknown);
            writer.Write(Type);
            writer.Write(Name.Length + 1);
            writer.Write(Name);
            writer.Write(UnkArray1);
            writer.Write(ValuesI[0]);
            writer.Write(ValuesI[1]);
            writer.Write(ValuesI[2]);
            writer.Write(ValuesI[3]);
            writer.Write(UnkArray2);
            writer.Write(ValuesF[0]);
            writer.Write(ValuesF[1]);
            writer.Write(ValuesF[2]);
            writer.Write(ValuesF[3]);
            writer.Write(Tex);
            writer.Write(Last);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Header = reader.ReadInt32();
            Unknown = reader.ReadInt32();
            Type = reader.ReadInt32();
            int Len = reader.ReadInt32();
            Name = new string(reader.ReadChars(Len - 1));
            reader.BaseStream.Position++;
            UnkArray1 = new byte[0x2A];
            UnkArray1 = reader.ReadBytes(0x2A);
            ValuesI[0] = reader.ReadInt32();
            ValuesI[1] = reader.ReadInt32();
            ValuesI[2] = reader.ReadInt32();
            ValuesI[3] = reader.ReadInt32();
            UnkArray2 = new byte[0x10];
            UnkArray2 = reader.ReadBytes(0x10);
            ValuesF[0] = reader.ReadSingle();
            ValuesF[1] = reader.ReadSingle();
            ValuesF[2] = reader.ReadSingle();
            ValuesF[3] = reader.ReadSingle();
            Tex = reader.ReadUInt32();
            Last = reader.ReadUInt32();
        }

        protected override int GetSize()
        {
            return 115 + Name.Length;
        }
    }
}

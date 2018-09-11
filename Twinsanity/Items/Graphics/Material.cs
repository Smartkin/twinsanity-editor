using System.IO;

namespace Twinsanity
{
    public class Material : TwinsItem
    {
        private string Name { get; set; }
        private int Header { get; set; }
        private int Unknown { get; set; }
        private int Type { get; set; }
        private uint Tex { get; set; }
        private uint Last { get; set; }
        private byte[] UnkArray1 { get; set; }
        private byte[] UnkArray2 { get; set; }
        private float[] ValuesF { get; set; } = new float[6];
        private int[] ValuesI { get; set; } = new int[2];


        public override void Save(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write(Unknown);
            writer.Write(Type);
            writer.Write(Name.Length + 1);
            writer.Write(Name);
            writer.Write(UnkArray1);
            writer.Write(ValuesF[0]);
            writer.Write(ValuesF[1]);
            writer.Write(ValuesF[2]);
            writer.Write(ValuesF[3]);
            writer.Write(UnkArray2);
            writer.Write(ValuesF[4]);
            writer.Write(ValuesF[0]);
            writer.Write(ValuesF[5]);
            writer.Write(ValuesF[1]);
            writer.Write(Tex);
            writer.Write(Last);
        }

        public override void Load(BinaryReader reader)
        {
            Header = reader.ReadInt32();
            Unknown = reader.ReadInt32();
            Type = reader.ReadInt32();
            int Len = reader.ReadInt32();
            Name = new string(reader.ReadChars(Len - 1));
            reader.BaseStream.Position++;
            UnkArray1 = new byte[0x2A];
            UnkArray1 = reader.ReadBytes(0x2A);
            ValuesF[0] = reader.ReadSingle();
            ValuesF[1] = reader.ReadSingle();
            ValuesF[2] = reader.ReadSingle();
            ValuesF[3] = reader.ReadSingle();
            UnkArray2 = new byte[0x10];
            UnkArray2 = reader.ReadBytes(0x10);
            ValuesF[4] = reader.ReadSingle();
            ValuesF[0] = reader.ReadInt32();
            ValuesF[5] = reader.ReadSingle();
            ValuesF[1] = reader.ReadInt32();
            Tex = reader.ReadUInt32();
            Last = reader.ReadUInt32();
        }

        protected override int GetSize()
        {
            return 115 + Name.Length;
        }
    }
}

using System.IO;

namespace Twinsanity
{
    public class Trigger : TwinsItem
    {
        public uint SomeUInt32 { get; set; }
        public uint SomeNumber { get; set; }
        public float SomeFloat { get; set; }
        public Pos[] Coords { get; set; } = new Pos[3];
        public uint SectionHead { get; set; }
        public ushort[] Instances { get; set; }
        public ushort SomeUInt161 { get; set; }
        public ushort SomeUInt162 { get; set; }
        public ushort SomeUInt163 { get; set; }
        public ushort SomeUInt164 { get; set; }


        public override void Save(BinaryWriter writer)
        {
            writer.Write(SomeUInt32);
            writer.Write(SomeNumber);
            writer.Write(SomeFloat);
            for (int i = 0; i < 3; ++i)
            {
                writer.Write(Coords[i].X);
                writer.Write(Coords[i].Y);
                writer.Write(Coords[i].Z);
                writer.Write(Coords[i].W);
            }
            writer.Write(Instances.Length);
            writer.Write(Instances.Length);
            writer.Write(SectionHead);
            for (int i = 0; i < Instances.Length; ++i)
                writer.Write(Instances[i]);
            writer.Write(SomeUInt161);
            writer.Write(SomeUInt162);
            writer.Write(SomeUInt163);
            writer.Write(SomeUInt164);
        }

        public override void Load(BinaryReader reader, int size)
        {
            SomeUInt32 = reader.ReadUInt32();
            SomeNumber = reader.ReadUInt32();
            SomeFloat = reader.ReadSingle();
            for (int i = 0; i < 3; ++i)
            {
                Coords[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            var n = reader.ReadInt32();
            n  = reader.ReadInt32();
            SectionHead = reader.ReadUInt32();
            Instances = new ushort[n];
            for (int i = 0; i < n; ++i)
                Instances[i] = reader.ReadUInt16();
            SomeUInt161 = reader.ReadUInt16();
            SomeUInt162 = reader.ReadUInt16();
            SomeUInt163 = reader.ReadUInt16();
            SomeUInt164 = reader.ReadUInt16();
        }

        protected override int GetSize()
        {
            return 80 + Instances.Length * 2;
        }
    }
}

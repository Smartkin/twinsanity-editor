using System;

namespace Twinsanity
{
    public class Trigger : BaseItem
    {
        public new string NodeName = "Trigger";
        public uint SomeUInt32;
        public uint SomeNumber;
        public uint SomeFlag;
        public RM2.Coordinate4[] Coordinate = new RM2.Coordinate4[4];
        public int SectionSize;
        public uint SectionHead;
        public ushort[] SomeUInt16;
        public ushort SomeUInt161;
        public ushort SomeUInt162;
        public ushort SomeUInt163;
        public ushort SomeUInt164;
        

        public override void UpdateStream()
        {
            System.IO.MemoryStream NewStream = new System.IO.MemoryStream();
            System.IO.BinaryWriter NSWriter = new System.IO.BinaryWriter(NewStream);
            NSWriter.Write(SomeUInt32);
            NSWriter.Write(SomeNumber);
            NSWriter.Write(SomeFlag);
            for (int i = 0; i <= 2; i++)
            {
                NSWriter.Write(Coordinate[i].X);
                NSWriter.Write(Coordinate[i].Y);
                NSWriter.Write(Coordinate[i].Z);
                NSWriter.Write(Coordinate[i].W);
            }
            NSWriter.Write(SectionSize);
            NSWriter.Write(SectionSize);
            NSWriter.Write(SectionHead);
            for (int i = 0; i <= SectionSize - 1; i++)
                NSWriter.Write(SomeUInt16[i]);
            NSWriter.Write(SomeUInt161);
            NSWriter.Write(SomeUInt162);
            NSWriter.Write(SomeUInt163);
            NSWriter.Write(SomeUInt164);
            ByteStream = NewStream;
            Size = (uint)ByteStream.Length;
        }

        protected override void DataUpdate()
        {
            System.IO.BinaryReader BSReader = new System.IO.BinaryReader(ByteStream);
            ByteStream.Position = 0;
            SomeUInt32 = BSReader.ReadUInt32();
            SomeNumber = BSReader.ReadUInt32();
            SomeFlag = BSReader.ReadUInt32();
            for (int i = 0; i <= 2; i++)
            {
                Coordinate[i].X = BSReader.ReadSingle();
                Coordinate[i].Y = BSReader.ReadSingle();
                Coordinate[i].Z = BSReader.ReadSingle();
                Coordinate[i].W = BSReader.ReadSingle();
            }
            SectionSize = BSReader.ReadInt32();
            SectionSize = BSReader.ReadInt32();
            SectionHead = BSReader.ReadUInt32();
            Array.Resize(ref SomeUInt16, SectionSize);
            for (int i = 0; i <= SectionSize - 1; i++)
                SomeUInt16[i] = BSReader.ReadUInt16();
            SomeUInt161 = BSReader.ReadUInt16();
            SomeUInt162 = BSReader.ReadUInt16();
            SomeUInt163 = BSReader.ReadUInt16();
            SomeUInt164 = BSReader.ReadUInt16();
        }
    }
}

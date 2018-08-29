using System;

namespace Twinsanity
{
    public class Behavior : BaseItem
    {
        public new string NodeName = "Behavior";
        public RM2.Coordinate4 Cord;
        public ushort Num;

        public override void UpdateStream()
        {
            System.IO.MemoryStream NewStream = new System.IO.MemoryStream();
            System.IO.BinaryWriter NSWriter = new System.IO.BinaryWriter(NewStream);
            NSWriter.Write(Cord.X);
            NSWriter.Write(Cord.Y);
            NSWriter.Write(Cord.Z);
            NSWriter.Write(Cord.W);
            NSWriter.Write(Num);
            ByteStream = NewStream;
            Size = (uint)ByteStream.Length;
        }

        protected override void DataUpdate()
        {
            System.IO.BinaryReader BSReader = new System.IO.BinaryReader(ByteStream);
            ByteStream.Position = 0;
            Cord.X = BSReader.ReadSingle();
            Cord.Y = BSReader.ReadSingle();
            Cord.Z = BSReader.ReadSingle();
            Cord.W = BSReader.ReadSingle();
            Num = BSReader.ReadUInt16();
        }
    }
}

using System;

namespace Twinsanity
{
    public class Position : BaseItem
    {
        public new string NodeName = "Position";
        public RM2.Coordinate4 Pos;

        public override void UpdateStream()
        {
            System.IO.MemoryStream NewStream = new System.IO.MemoryStream();
            System.IO.BinaryWriter NSWriter = new System.IO.BinaryWriter(NewStream);
            NSWriter.Write(Pos.X);
            NSWriter.Write(Pos.Y);
            NSWriter.Write(Pos.Z);
            NSWriter.Write(Pos.W);
            ByteStream = NewStream;
            Size = (uint)ByteStream.Length;
        }

        protected override void DataUpdate()
        {
            System.IO.BinaryReader BSReader = new System.IO.BinaryReader(ByteStream);
            ByteStream.Position = 0;
            Pos.X = BSReader.ReadSingle();
            Pos.Y = BSReader.ReadSingle();
            Pos.Z = BSReader.ReadSingle();
            Pos.W = BSReader.ReadSingle();
        }
    }
}

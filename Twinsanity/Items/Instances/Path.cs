using System;

namespace Twinsanity
{
    public class Path : BaseItem
    {
        public new string NodeName = "Path";
        public RM2.Coordinate4[] Pos = new RM2.Coordinate4[] { };
        public Something[] SomeInts = new Something[] { };

        public override void UpdateStream()
        {
            System.IO.MemoryStream NewStream = new System.IO.MemoryStream();
            System.IO.BinaryWriter NSWriter = new System.IO.BinaryWriter(NewStream);
            NSWriter.Write(Pos.Length);
            for (int i = 0; i <= Pos.Length - 1; i++)
            {
                NSWriter.Write(Pos[i].X);
                NSWriter.Write(Pos[i].Y);
                NSWriter.Write(Pos[i].Z);
                NSWriter.Write(Pos[i].W);
            }
            NSWriter.Write(SomeInts.Length);
            for (int i = 0; i <= SomeInts.Length - 1; i++)
            {
                NSWriter.Write(SomeInts[i].Int1);
                NSWriter.Write(SomeInts[i].Int2);
            }
            ByteStream = NewStream;
            Size = (uint)ByteStream.Length;
        }

        protected override void DataUpdate()
        {
            System.IO.BinaryReader BSReader = new System.IO.BinaryReader(ByteStream);
            ByteStream.Position = 0;
            Array.Resize(ref Pos, BSReader.ReadInt32());
            for (int i = 0; i <= Pos.Length - 1; i++)
            {
                Pos[i].X = BSReader.ReadSingle();
                Pos[i].Y = BSReader.ReadSingle();
                Pos[i].Z = BSReader.ReadSingle();
                Pos[i].W = BSReader.ReadSingle();
            }
            Array.Resize(ref SomeInts, BSReader.ReadInt32());
            for (int i = 0; i <= SomeInts.Length - 1; i++)
            {
                SomeInts[i].Int1 = BSReader.ReadUInt32();
                SomeInts[i].Int2 = BSReader.ReadUInt32();
            }
        }

        #region STRUCTURES
        public struct Something
        {
            public uint Int1;
            public uint Int2;
        }
        #endregion
    }
}

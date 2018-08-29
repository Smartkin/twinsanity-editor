using System;

namespace Twinsanity
{
    public class FuckingShit : BaseItem
    {
        public new string NodeName = "FuckingShit";
        public ushort[] I16 = new ushort[5];

        public override void UpdateStream()
        {
            System.IO.MemoryStream NewStream = new System.IO.MemoryStream();
            System.IO.BinaryWriter NSWriter = new System.IO.BinaryWriter(NewStream);
            NSWriter.Write(I16[0]);
            NSWriter.Write(I16[1]);
            NSWriter.Write(I16[2]);
            NSWriter.Write(I16[3]);
            NSWriter.Write(I16[4]);
            ByteStream = NewStream;
            Size = (uint)ByteStream.Length;
        }

        protected override void DataUpdate()
        {
            System.IO.BinaryReader BSReader = new System.IO.BinaryReader(ByteStream);
            ByteStream.Position = 0;
            I16[0] = BSReader.ReadUInt16();
            I16[1] = BSReader.ReadUInt16();
            I16[2] = BSReader.ReadUInt16();
            I16[3] = BSReader.ReadUInt16();
            I16[4] = BSReader.ReadUInt16();
        }

    }
}

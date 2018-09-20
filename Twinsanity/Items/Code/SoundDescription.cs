namespace Twinsanity
{
    public class SoundDescription : BaseItem
    {
        public new string NodeName = "SoundDescription";

        public uint Head;
        public ushort Frequency;
        public ushort Param1;
        public ushort Param2;
        public ushort Param3;
        public ushort Param4;
        public uint SoundSize;
        public uint SoundOffset;

        public override void UpdateStream()
        {
            System.IO.MemoryStream NewStream = new System.IO.MemoryStream();
            System.IO.BinaryWriter NSWriter = new System.IO.BinaryWriter(NewStream);
            NSWriter.Write(Head);
            NSWriter.Write(Frequency);
            NSWriter.Write(Param1);
            NSWriter.Write(Param2);
            NSWriter.Write(Param3);
            NSWriter.Write(Param4);
            NSWriter.Write(SoundSize);
            NSWriter.Write(SoundOffset);
            ByteStream = NewStream;
            Size = (uint)ByteStream.Length;
        }

        protected override void DataUpdate()
        {
            System.IO.BinaryReader BSReader = new System.IO.BinaryReader(ByteStream);
            ByteStream.Position = 0;
            Head = BSReader.ReadUInt32();
            Frequency = BSReader.ReadUInt16();
            Param1 = BSReader.ReadUInt16();
            Param2 = BSReader.ReadUInt16();
            Param3 = BSReader.ReadUInt16();
            Param4 = BSReader.ReadUInt16();
            SoundSize = BSReader.ReadUInt32();
            SoundOffset = BSReader.ReadUInt32();
        }
    }
}

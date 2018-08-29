using System;

namespace Twinsanity
{
    public class Script : BaseItem
    {
        public new string NodeName = "Script";

        public ushort InsideID = 0;
        public byte Mask = 0;
        public byte Flag = 0;
        public string Name = "";
        public byte[] ScriptArray = new byte[] { 0, 0, 0, 0 };

        public override void UpdateStream()
        {
            System.IO.MemoryStream NewStream = new System.IO.MemoryStream();
            System.IO.BinaryWriter NSWriter = new System.IO.BinaryWriter(NewStream);
            NSWriter.Write(InsideID);
            NSWriter.Write(Mask);
            NSWriter.Write(Flag);
            if (Flag == 0)
            {
                NSWriter.Write(Name.Length);
                for (int i = 0; i <= Name.Length - 1; i++)
                    NSWriter.Write(Name[i]);
            }
            NSWriter.Write(ScriptArray);
            ByteStream = NewStream;
            Size = (uint)ByteStream.Length;
        }

        protected override void DataUpdate()
        {
            System.IO.BinaryReader BSReader = new System.IO.BinaryReader(ByteStream);
            ByteStream.Position = 0;
            InsideID = BSReader.ReadUInt16();
            Mask = BSReader.ReadByte();
            Flag = BSReader.ReadByte();
            if (Flag == 0)
            {
                int len = BSReader.ReadInt32();
                Name = new string(BSReader.ReadChars(len));
            }
            ScriptArray = BSReader.ReadBytes((int)(ByteStream.Length - ByteStream.Position));
        }
    }
}

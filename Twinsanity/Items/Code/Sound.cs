using System;

namespace Twinsanity
{
    public class Sound : BaseItem
    {
        public int Shift = 0;
        public new string NodeName = "Sound";

        public override uint Recalculate()
        {
            UpdateStream();
            return (uint)(ByteStream.Length + Shift);
        }

        public override void Load(ref System.IO.FileStream File, ref System.IO.BinaryReader Reader)
        {
            File.Position = Offset + Base + Shift;
            System.IO.BinaryWriter BSWriter = new System.IO.BinaryWriter(ByteStream);
            BSWriter.Write(Reader.ReadBytes((int)Size), 0, (int)(Size - Shift));
            DataUpdate();
        }

        public override void UpdateStream()
        {
            System.IO.MemoryStream NewStream = new System.IO.MemoryStream();
            System.IO.BinaryWriter NSWriter = new System.IO.BinaryWriter(NewStream);
            NSWriter.Write(ByteStream.ToArray());
            ByteStream = NewStream;
            Size = (uint)(ByteStream.Length + Shift);
        }
    }
}

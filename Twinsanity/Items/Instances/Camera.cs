using System.IO;
using System.Collections.Generic;

namespace Twinsanity
{
    public class Camera : TwinsItem
    {
        public uint Header { get; set; }
        public int Enabled { get; set; }
        public float UnkFac1 { get; set; }
        public Pos TriggerRot { get; set; }
        public Pos TriggerPos { get; set; }
        public Pos TriggerSize { get; set; }
        public int Int0 { get; set; }
        public int Int1 { get; set; }
        public int Int2 { get; set; }
        public CamRot CamRot { get; set; }
        public float UnkFac2 { get; set; }
        public byte[] Unknown { get; set; } = new byte[0x60];
        public int SecHeader1 { get; set; }
        public int SecHeader2 { get; set; }
        public byte Type { get; set; }

        //INCOMPLETE

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write(Enabled);
            writer.Write(UnkFac1);
            writer.Write(TriggerRot.X); writer.Write(TriggerRot.Y); writer.Write(TriggerRot.Z); writer.Write(TriggerRot.W);
            writer.Write(TriggerPos.X); writer.Write(TriggerPos.Y); writer.Write(TriggerPos.Z); writer.Write(TriggerPos.W);
            writer.Write(TriggerSize.X); writer.Write(TriggerSize.Y); writer.Write(TriggerSize.Z); writer.Write(TriggerSize.W);
            writer.Write(Int0);
            writer.Write(Int1);
            writer.Write(Int2);
            writer.Write(CamRot.Pitch); writer.Write(CamRot.Yaw); writer.Write(CamRot.Roll);
            writer.Write(UnkFac2);
            writer.Write(Unknown);
            writer.Write(SecHeader1);
            writer.Write(SecHeader2);
            writer.Write(Type);
            base.Save(writer);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Header = reader.ReadUInt32();
            Enabled = reader.ReadInt32();
            UnkFac1 = reader.ReadSingle();
            TriggerRot = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            TriggerPos = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            TriggerSize = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Int0 = reader.ReadInt32();
            Int1 = reader.ReadInt32();
            Int2 = reader.ReadInt32();
            CamRot = new CamRot(reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16());
            UnkFac2 = reader.ReadSingle();
            Unknown = reader.ReadBytes(0x60);
            SecHeader1 = reader.ReadInt32();
            SecHeader2 = reader.ReadInt32();
            Type = reader.ReadByte();
            base.Load(reader, size-0xBB);
        }
    }
}

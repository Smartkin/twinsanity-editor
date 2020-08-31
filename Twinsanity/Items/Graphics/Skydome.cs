using System.IO;

namespace Twinsanity
{
    public class Skydome : TwinsItem
    {
        public uint Unknown { get; set; }
        public uint[] ModelIDs { get; set; }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Unknown);
            writer.Write(ModelIDs.Length);
            for (int i = 0; i < ModelIDs.Length; ++i)
                writer.Write(ModelIDs[i]);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Unknown = reader.ReadUInt32();
            var count = reader.ReadInt32();
            ModelIDs = new uint[count];
            for (int i = 0; i < count; ++i)
                ModelIDs[i] = reader.ReadUInt32();
        }
    }
}

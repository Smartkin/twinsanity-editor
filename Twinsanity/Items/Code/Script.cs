using System.IO;

namespace Twinsanity
{
    public class Script : TwinsItem
    {
        public string Name { get; set; }

        private ushort id;
        private byte mask;
        private byte flag;
        private byte[] script;

        public override void Save(BinaryWriter writer)
        {
            writer.Write(id);
            writer.Write(mask);
            writer.Write(flag);
            if (flag == 0)
            {
                writer.Write(Name.Length);
                writer.Write(Name.ToCharArray(), 0, Name.Length);
            }
            writer.Write(script);
        }

        public override void Load(BinaryReader reader, int size)
        {
            var sk = reader.BaseStream.Position;
            id = reader.ReadUInt16();
            mask = reader.ReadByte();
            flag = reader.ReadByte();
            if (flag == 0)
            {
                int len = reader.ReadInt32();
                Name = new string(reader.ReadChars(len));
            }
            script = reader.ReadBytes(size - (int)(reader.BaseStream.Position - sk));
        }

        protected override int GetSize()
        {
            return 4 + script.Length + (flag == 0 ? 4 + Name.Length : 0);
        }
    }
}

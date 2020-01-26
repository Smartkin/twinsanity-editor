using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public class Script : TwinsItem
    {
        private struct HeaderScript
        {
            public struct UnkIntPairs
            {
                public uint unkInt1;
                public uint unkInt2;
            }
            public uint unkIntPairs;
            public UnkIntPairs[] pairs;
        }

        public string Name { get; set; }

        private ushort id;
        private byte mask;
        private byte flag;
        private HeaderScript headerScript;
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
            else
            {
                writer.Write(headerScript.unkIntPairs);
                for (int i = 0; i < headerScript.unkIntPairs; i++)
                {
                    writer.Write(headerScript.pairs[i].unkInt1);
                    writer.Write(headerScript.pairs[i].unkInt2);
                }
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
            else
            {
                Name = "Header script";
                headerScript.unkIntPairs = reader.ReadUInt32();
                headerScript.pairs = new HeaderScript.UnkIntPairs[headerScript.unkIntPairs];
                for (int i = 0; i < headerScript.unkIntPairs; i++)
                {
                    headerScript.pairs[i].unkInt1 = reader.ReadUInt32();
                    headerScript.pairs[i].unkInt2 = reader.ReadUInt32();
                }
            }
            script = reader.ReadBytes(size - (int)(reader.BaseStream.Position - sk));
        }

        protected override int GetSize()
        {
            if (flag != 0)
            {
                return 4 + 4 + headerScript.pairs.Length * 8;
            }
            return 4 + script.Length +  4 + Name.Length;
        }
    }
}

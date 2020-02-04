using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public class Script : TwinsItem
    {
        public class HeaderScriptStruct
        {
            public struct UnkIntPairs
            {
                public int mainScriptIndex;
                public uint unkInt2;
            }
            public uint unkIntPairs;
            public UnkIntPairs[] pairs;
        }

        public string Name { get; set; }

        private ushort id;
        private byte mask;
        private byte flag;
        public HeaderScriptStruct HeaderScript { get; set; }
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
                writer.Write(HeaderScript.unkIntPairs);
                for (int i = 0; i < HeaderScript.unkIntPairs; i++)
                {
                    writer.Write(HeaderScript.pairs[i].mainScriptIndex);
                    writer.Write(HeaderScript.pairs[i].unkInt2);
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
                HeaderScript = new HeaderScriptStruct();
                HeaderScript.unkIntPairs = reader.ReadUInt32();
                HeaderScript.pairs = new HeaderScriptStruct.UnkIntPairs[HeaderScript.unkIntPairs];
                for (int i = 0; i < HeaderScript.unkIntPairs; i++)
                {
                    HeaderScript.pairs[i].mainScriptIndex = reader.ReadInt32();
                    HeaderScript.pairs[i].unkInt2 = reader.ReadUInt32();
                }
            }
            script = reader.ReadBytes(size - (int)(reader.BaseStream.Position - sk));
        }

        protected override int GetSize()
        {
            if (flag != 0)
            {
                return 4 + 4 + HeaderScript.pairs.Length * 8;
            }
            return 4 + script.Length +  4 + Name.Length;
        }
    }
}

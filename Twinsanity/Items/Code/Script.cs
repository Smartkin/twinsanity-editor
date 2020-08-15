using System;
using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public class Script : TwinsItem
    {
        public class HeaderScriptStruct
        {
            public HeaderScriptStruct(BinaryReader reader)
            {
                unkIntPairs = reader.ReadUInt32();
                pairs = new UnkIntPairs[unkIntPairs];
                for (int i = 0; i < unkIntPairs; i++)
                {
                    pairs[i].mainScriptIndex = reader.ReadInt32();
                    pairs[i].unkInt2 = reader.ReadUInt32();
                }
            }
            public struct UnkIntPairs
            {
                public int mainScriptIndex;
                public uint unkInt2;
            }
            public uint unkIntPairs;
            public UnkIntPairs[] pairs;
        }

        public class MainScriptStruct
        {
            public MainScriptStruct(BinaryReader reader)
            {
                int len = reader.ReadInt32();
                name = new string(reader.ReadChars(len));
            }
            public String name { get; set; }
            public Int32 unkInt1 { get; set; }
            public Int32 unkInt2 { get; set; }
            public LinkedScript linkedScript1 { get; set; }
            public LinkedScript linkedScript2 { get; set; }

            public class SupportType1
            {
                public SupportType1(BinaryReader reader)
                {

                }
                public byte unkByte1 { get; set; }
                public byte unkByte2 { get; set; }
                public UInt16 unkUShort1 { get; set; }
                public Int32 unkInt1 { get; set; }
                public Byte[] byteArray { get; set; }
            }
            public class SupportType2
            {
                public SupportType2(BinaryReader reader)
                {

                }
                public Int32 bitfield { get; set; }
                public LinkedScript linkedScript { get; set; }
                public SupportType3 type3 { get; set; }
                public SupportType4 type4 { get; set; }
                public SupportType2 nextType2 { get; set; }
            }
            public class SupportType3
            {
                public SupportType3(BinaryReader reader)
                {

                }
                public Int32 unkInt1 { get; set; }
                public Int32 vTableAddress { get; set; }
                public float X { get; set; }
                public float Y { get; set; }
                public float Z { get; set; }
            }
            public class SupportType4
            {
                public SupportType4(BinaryReader reader)
                {

                }
                public UInt32 unkUInt { get; set; }
                public Int32 vTableAddress { get; set; }
                public Int32 internalIndex { get; set; }
                public Int32 length { get; set; }
                public Byte[] byteArray { get; set; }
                public SupportType4 nextType4 { get; set; }
            }
            public class LinkedScript
            {
                public LinkedScript(BinaryReader reader)
                {

                }
                public Int16 bitfield { get; set; }
                public Int16 scriptIndexOrSlot { get; set; }
                public SupportType1 type1 { get; set; }
                public SupportType2 type2 { get; set; }
                public LinkedScript nextLinked { get; set; }
            }
        }

        public string Name { get; set; }

        private ushort id;
        private byte mask;
        private byte flag;
        public HeaderScriptStruct HeaderScript { get; set; }
        public MainScriptStruct MainScript { get; set; }
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
                MainScript = new MainScriptStruct(reader);
                Name = MainScript.name;
            }
            else
            {
                Name = "Header script";
                HeaderScript = new HeaderScriptStruct(reader);
                
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

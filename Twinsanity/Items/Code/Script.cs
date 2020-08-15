using System;
using System.Collections.Generic;
using System.Drawing;
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
                LinkedScriptsCount = reader.ReadInt32();
                unkInt2 = reader.ReadInt32();
                linkedScript1 = new LinkedScript(reader);
                LinkedScript ptr = linkedScript1;
                while (ptr != null)
                {
                    if ((ptr.bitfield & 0x1F) != 0)
                    {
                        ptr.type2 = new SupportType2(reader);
                    }
                    ptr = ptr.nextLinked;
                }
            }
            public String name { get; set; }
            public Int32 LinkedScriptsCount { get; set; }
            public Int32 unkInt2 { get; set; }
            public LinkedScript linkedScript1 { get; set; }
            public LinkedScript linkedScript2 { get; set; }

            public class SupportType1
            {
                public SupportType1(BinaryReader reader)
                {
                    unkByte1 = reader.ReadByte();
                    unkByte2 = reader.ReadByte();
                    unkUShort1 = reader.ReadUInt16();
                    unkInt1 = reader.ReadInt32();
                    Int32 byteArrayLen = unkByte1 + unkByte2 * 4;
                    byteArray = reader.ReadBytes(unkByte1 + unkByte2 * 4);
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
                    bitfield = reader.ReadInt32();
                    if ((bitfield & 0x400) != 0)
                    {
                        linkedScriptListIndex = reader.ReadInt32();
                    }
                    if ((bitfield & 0x200) != 0)
                    {
                        type3 = new SupportType3(reader);
                    }
                    if ((bitfield & 0xFF) != 0)
                    {
                        type4 = new SupportType4(reader);
                    }
                    if ((bitfield & 0x800) != 0)
                    {
                        nextType2 = new SupportType2(reader);
                    }
                }
                public Int32 bitfield { get; set; }
                public Int32 linkedScriptListIndex { get; set; }
                public SupportType3 type3 { get; set; }
                public SupportType4 type4 { get; set; }
                public SupportType2 nextType2 { get; set; }
            }
            public class SupportType3
            {
                public SupportType3(BinaryReader reader)
                {
                    unkInt1 = reader.ReadInt32();
                    vTableAddress = 0x0;
                    X = reader.ReadSingle();
                    Y = reader.ReadSingle();
                    Z = reader.ReadSingle();
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
                    internalIndex = reader.ReadInt32();
                    length = GetSupport4Size(internalIndex & 0x0000FFFF);
                    if (length - 0xC > 0x0)
                    {
                        byteArray = reader.ReadBytes(length - 0xC);
                    }
                    if ((internalIndex & 0x1000000) != 0)
                    {
                        nextType4 = new SupportType4(reader);
                        int flag = (length != 0) ? 1 : 0;
                        unkUInt = (UInt32)(((UInt64)unkUInt & 0xFEFFFFFF) | (UInt64)flag << 0x18);
                    }
                    else
                    {
                        unkUInt &= 0xFEFFFFFF;
                    }

                }
                public UInt32 unkUInt { get; set; }
                public Int32 vTableAddress { get; set; }
                public Int32 internalIndex { get; set; }
                public Int32 length { get; set; }
                public Byte[] byteArray { get; set; }
                public SupportType4 nextType4 { get; set; }

                private static int GetSupport4Size(int support4Index)
                {
                    switch (support4Index)
                    {
                        case 0x0:
                            return 0x0;
                        case 0x1:
                            return 0x80;
                        case 0x2:
                            return 0xC;
                        case 0x3:
                            return 0x20;
                        case 0x4:
                            return 0x10;
                        case 0x5:
                            return 0xC;
                        case 0x6:
                            return 0x0;
                        case 0x7:
                            return 0xC;
                        case 0x8:
                            return 0x30;
                        case 0x9:
                            return 0x24;
                        case 0xA:
                            return 0x30;
                        case 0xB:
                            return 0x48;
                        case 0xC:
                            return 0x94;
                        case 0xD:
                            return 0xC;
                        case 0xE:
                            return 0x10;
                        case 0xF:
                            return 0x10;
                        case 0x10:
                            return 0x10;
                        case 0x11:
                            return 0x10;
                        case 0x12:
                            return 0x10;
                        case 0x13:
                            return 0x10;
                        case 0x14:
                        case 0x15:
                        case 0x16:
                            return 0x0;
                        case 0x17:
                            return 0x10;
                        case 0x18:
                            return 0x10;
                        case 0x19:
                        case 0x1A:
                            return 0x0;
                        case 0x1B:
                            return 0x10;
                        case 0x1C:
                            return 0x10;
                        case 0x1D:
                            return 0x20;
                        case 0x1E:
                            return 0x0;
                        case 0x1F:
                            return 0x10;
                        case 0x20:
                            return 0x0;
                        case 0x21:
                            return 0x10;
                        case 0x22:
                            return 0x10;
                        case 0x23:
                            return 0xC;
                        case 0x24:
                            return 0xC;
                        case 0x25:
                        case 0x26:
                            return 0x0;
                        case 0x27:
                            return 0xC;
                        case 0x28:
                            return 0x14;
                        case 0x29:
                            return 0x0;
                        case 0x2A:
                            return 0x10;
                        case 0x2B:
                            return 0x0;
                        case 0x2C:
                            return 0x50;
                        case 0x2D:
                            return 0x10;
                        case 0x2E:
                            return 0x0;
                        case 0x2F:
                            return 0x30;
                        case 0x30:
                            return 0x30;
                        case 0x31:
                            return 0x30;
                        case 0x32:
                            return 0xC;
                        case 0x33:
                            return 0x20;
                        case 0x34:
                            return 0xC;
                        case 0x35:
                            return 0xC;
                        case 0x36:
                            return 0x1C;
                        case 0x58:
                            return 0x1C;
                        case 0x72:
                        case 0x73:
                            return 0xC;
                        case 0x75:
                            return 0x14;
                        case 0x8B:
                            return 0x10;
                        case 0x98:
                            return 0x18;
                        case 0x213:
                            return 0xC;
                        default:
                            return 0x10; //Return 0x10 by default for now even though it's supposed to be 0x0 :P
                    }
                }

            }
            public class LinkedScript
            {
                public LinkedScript(BinaryReader reader)
                {
                    bitfield = reader.ReadInt16();
                    scriptIndexOrSlot = reader.ReadInt16();
                    if ((bitfield & 0x4000) != 0)
                    {
                        type1 = new SupportType1(reader);
                    }
                    if ((bitfield & 0x8000) != 0)
                    {
                        nextLinked = new LinkedScript(reader);
                    }
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
